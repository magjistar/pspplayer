// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "KernelHandle.h"
#include "KernelCallback.h"
#include "KernelEvent.h"
#include "KernelPartition.h"
#include "KernelInterruptHandler.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;
using namespace System::Runtime::CompilerServices;
using namespace System::Threading;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Games;
using namespace Noxa::Emulation::Psp::Media;
using namespace Noxa::Emulation::Psp::Utilities;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				ref class FastBios;
				ref class KernelDevice;
				ref class KernelFileHandle;
				ref class KernelPartition;
				ref class KernelStatistics;
				ref class KernelThread;

				ref class Kernel : public IKernel
				{
				internal:
					FastBios^							_bios;
					IEmulationInstance^					_emu;
					ICpu^								_cpu;
					ICpuCore^							_core0;
					GameInformation^					_game;
					AutoResetEvent^						_gameEvent;
					Stream^								_bootStream;

					int									_lastId;
					Dictionary<int, KernelHandle^>^		_handles;

					KernelThread^						_activeThread;
					List<KernelThread^>^				_threadsWaitingOnEvents;
					List<KernelThread^>^				_delayedThreads;
					Timers::Timer^						_delayedThreadTimer;	// Set to go off at the time that the next thread should wake up

					List<KernelDevice^>^				_devices;
					Dictionary<String^, KernelDevice^>^	_deviceMap;

				public:
					PerformanceTimer^					Timer;
					double								StartTime;
					int64								StartTick;
					DateTime							StartDateTime;
					DateTime							UnixBaseTime;

					KernelStatistics^					Statistics;
					int64								IdleClocks;
					int									Status;

					Dictionary<int, KernelThread^>^		Threads;
					array<KernelPartition^>^			Partitions;

					KernelFileHandle^					StdIn;
					KernelFileHandle^					StdOut;
					KernelFileHandle^					StdErr;

					IMediaFolder^						CurrentPath;

					Dictionary<KernelCallbackType, KernelCallback^>^	Callbacks;
					array<array<KernelInterruptHandler^>^>^				InterruptHandlers;

				public:
					Kernel( FastBios^ bios );

					property GameInformation^ Game
					{
						virtual GameInformation^ get();
						virtual void set( GameInformation^ value );
					}

					property Stream^ BootStream
					{
						virtual Stream^ get()
						{
							return _bootStream;
						}
						virtual void set( Stream^ value )
						{
							_bootStream = value;
						}
					}

					property KernelThread^ ActiveThread
					{
						KernelThread^ get()
						{
							return _activeThread;
						}
					}

					void StartGame();
					void ExitGame( int status );

					virtual void Execute();

					void AddHandle( KernelHandle^ handle );
					void RemoveHandle( KernelHandle^ handle );

					__inline KernelHandle^ FindHandle( int id );
					__inline KernelDevice^ FindDevice( String^ path );

				public:
					void CreateThread( KernelThread^ thread );
					void DeleteThread( KernelThread^ thread );
					__inline KernelThread^ FindThread( int id );
					void WaitThreadOnEvent( KernelThread^ thread, KernelEvent^ ev, KernelThreadWaitTypes waitType, int bitMask, int outAddress );
					void SignalEvent( KernelEvent^ ev );
					void SpawnDelayedThreadTimer( int64 targetTick );

					[MethodImpl( MethodImplOptions::Synchronized )]
					void ContextSwitch();

				public:
					/// <summary>
					/// Unix time since 1970-01-01 UTC (not accurate) in microseconds.
					/// </summary>
					property int64 ClockTime
					{
						int64 get()
						{
							// 1000000 us per second
							// 10000000 ticks per second
							TimeSpan elapsed = DateTime::UtcNow - UnixBaseTime;
							return ( elapsed.Ticks / 10 );
						}
					}

					property double RunTime
					{
						double get()
						{
							return Timer->Elapsed - StartTime;
						}
					}

				private:
					void CreateStdio();
					void DelayedThreadTimerElapsed( Object^ sender, Timers::ElapsedEventArgs^ e );

				internal:
					int ThreadPriorityComparer( KernelThread^ a, KernelThread^ b );
					int ThreadDelayComparer( KernelThread^ a, KernelThread^ b );

				internal:
					int AllocateID()
					{
						return _lastId++;
					}
				};

			}
		}
	}
}
