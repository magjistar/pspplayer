// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "Stdafx.h"
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "sceCtrl.h"
#include "Kernel.h"

using namespace System;
using namespace System::Diagnostics;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Bios::Modules;
using namespace Noxa::Emulation::Psp::Input;

static uint _makedButtons;
static uint _breakedButtons;
static uint _pressedButtons;
static uint _releasedButtons;

int sceCtrlPeekLatchN( MemorySystem* memory, int latch_data );
int sceCtrlReadLatchN( MemorySystem* memory, int latch_data );

void* sceCtrl::QueryNativePointer( uint nid )
{
	switch( nid )
	{
	case 0xB1D0E5CD:
		return &sceCtrlPeekLatchN;
	case 0x0B588501:
		return &sceCtrlReadLatchN;
	};

	return 0;
}

void sceCtrl::Start()
{
	if( _threadRunning == true )
		this->Stop();

	_threadRunning = true;
	_thread = gcnew Thread( gcnew ThreadStart( this, &sceCtrl::InputThread ) );
	_thread->IsBackground = true;
	_thread->Name = "Kernel Input Thread";
	_thread->Priority = ThreadPriority::Normal;
	_thread->Start();
}

void sceCtrl::Stop()
{
	_threadRunning = false;
	if( _thread != nullptr )
		_thread->Join();
	_thread = nullptr;
}

void sceCtrl::Clear()
{
	if( _threadRunning == true )
		this->Stop();

	_sampleCycle = 0;
	_sampleMode = ControlSamplingMode::AnalogAndDigital;

	_makedButtons = 0;
	_breakedButtons = 0;
	_pressedButtons = 0;
	_releasedButtons = 0;
}

void sceCtrl::UpdateButtons( PadButtons buttons )
{
	// Set pressed buttons list
	uint oldPressed = _pressedButtons;
	_pressedButtons = ( uint )buttons;
	uint stillPressed = _pressedButtons & oldPressed;
	_releasedButtons = ~_pressedButtons;

	// Set maked/breaked (pressed/released) buttons this last sample
	_makedButtons = _pressedButtons & ~stillPressed;
	_breakedButtons = oldPressed & ~stillPressed;
}

void sceCtrl::InputThread()
{
	try
	{
		IEmulationInstance^ emu = _kernel->Emu;
		_device = emu->Input;
		Debug::Assert( _device != nullptr );
		if( _device == nullptr )
		{
			_threadRunning = false;
			return;
		}

		while( _threadRunning == true )
		{
			_device->Poll();
			PadButtons buttons = _device->Buttons;
			this->UpdateButtons( buttons );

			Thread::Sleep( InputPollInterval );
		}
	}
	catch( ThreadAbortException^ )
	{
	}
	catch( ThreadInterruptedException^ )
	{
	}
}

// int sceCtrlSetSamplingCycle(int cycle); (/ctrl/pspctrl.h:119)
int sceCtrl::sceCtrlSetSamplingCycle( int cycle )
{
	int old = _sampleCycle;
	_sampleCycle = cycle;
	Debug::WriteLine( String::Format( "sceCtrlSetSamplingCycle: set to {0} (was {1})", cycle, old ) );

	return old;
}

// int sceCtrlGetSamplingCycle(int *pcycle); (/ctrl/pspctrl.h:128)
int sceCtrl::sceCtrlGetSamplingCycle( IMemory^ memory, int pcycle )
{
	if( pcycle != 0x0 )
		memory->WriteWord( pcycle, 4, _sampleCycle );

	return 0;
}

// int sceCtrlSetSamplingMode(int mode); (/ctrl/pspctrl.h:137)
int sceCtrl::sceCtrlSetSamplingMode( int mode )
{
	int old = ( int )_sampleMode;
	_sampleMode = ( ControlSamplingMode )mode;

	return old;
}

// int sceCtrlGetSamplingMode(int *pmode); (/ctrl/pspctrl.h:146)
int sceCtrl::sceCtrlGetSamplingMode( IMemory^ memory, int pmode )
{
	if( pmode != 0x0 )
		memory->WriteWord( pmode, 4, ( int )_sampleMode );

	return 0;
}

// int sceCtrlPeekBufferPositive(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:148)
int sceCtrl::sceCtrlPeekBufferPositive( IMemory^ memory, int pad_data, int count )
{
	if( _device == nullptr )
		return 0;

	if( pad_data != 0x0 )
	{
		byte* p = MSI( memory )->Translate( pad_data );
		for( int n = 0; n < count; n++ )
		{
			_device->Poll();
			PadButtons buttons = _device->Buttons;
			this->UpdateButtons( buttons );
			int analogX = _device->AnalogX;
			int analogY = _device->AnalogY;
			float max = ushort::MaxValue;
			//if( analogX == 0 )
				analogX = ( int )( ( ( ( float )analogX / max ) + 0.5f ) * 256 );
			//if( analogY == 0 )
				analogY = ( int )( ( ( ( float )analogY / max ) + 0.5f ) * 256 );

			*( ( int* )p ) = ( int )Environment::TickCount;
			*( ( int* )( p + 4 ) ) = ( int )buttons;
			*( p + 8 ) = ( byte )analogX;
			*( p + 9 ) = ( byte )analogY;
			// 6 bytes of junk
			p += 16;
		}
	}

	return count;
}

// int sceCtrlPeekBufferNegative(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:150)
int sceCtrl::sceCtrlPeekBufferNegative( IMemory^ memory, int pad_data, int count )
{
	// Same as above, but complement button mask

	if( _device == nullptr )
		return 0;

	if( pad_data != 0x0 )
	{
		byte* p = MSI( memory )->Translate( pad_data );
		for( int n = 0; n < count; n++ )
		{
			_device->Poll();
			PadButtons buttons = _device->Buttons;
			this->UpdateButtons( buttons );
			int analogX = _device->AnalogX;
			int analogY = _device->AnalogY;
			float max = ushort::MaxValue;
			if( analogX == 0 )
				analogX = ( int )( ( ( ( float )analogX / max ) + 0.5f ) * 256 );
			if( analogY == 0 )
				analogY = ( int )( ( ( ( float )analogY / max ) + 0.5f ) * 256 );

			*( ( int* )p ) = ( int )Environment::TickCount;
			*( ( int* )( p + 4 ) ) = ~( int )buttons;
			*( p + 8 ) = ( byte )analogX;
			*( p + 9 ) = ( byte )analogY;
			// 6 bytes of junk
			p += 16;
		}
	}

	return count;
}

// int sceCtrlReadBufferPositive(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:168)
int sceCtrl::sceCtrlReadBufferPositive( IMemory^ memory, int pad_data, int count )
{
	if( _device == nullptr )
		return 0;

	if( pad_data != 0x0 )
	{
		byte* p = MSI( memory )->Translate( pad_data );
		for( int n = 0; n < count; n++ )
		{
			_device->Poll();
			PadButtons buttons = _device->Buttons;
			this->UpdateButtons( buttons );
			int analogX = _device->AnalogX;
			int analogY = _device->AnalogY;
			float max = ushort::MaxValue;
			if( analogX == 0 )
				analogX = ( int )( ( ( ( float )analogX / max ) + 0.5f ) * 256 );
			if( analogY == 0 )
				analogY = ( int )( ( ( ( float )analogY / max ) + 0.5f ) * 256 );

			*( ( int* )p ) = ( int )Environment::TickCount;
			*( ( int* )( p + 4 ) ) = ( int )buttons;
			*( p + 8 ) = ( byte )analogX;
			*( p + 9 ) = ( byte )analogY;
			// 6 bytes of junk
			p += 16;
		}
	}

	return count;
}

// int sceCtrlReadBufferNegative(SceCtrlData *pad_data, int count); (/ctrl/pspctrl.h:170)
int sceCtrl::sceCtrlReadBufferNegative( IMemory^ memory, int pad_data, int count )
{
	if( _device == nullptr )
		return 0;

	if( pad_data != 0x0 )
	{
		byte* p = MSI( memory )->Translate( pad_data );
		for( int n = 0; n < count; n++ )
		{
			_device->Poll();
			PadButtons buttons = _device->Buttons;
			this->UpdateButtons( buttons );
			int analogX = _device->AnalogX;
			int analogY = _device->AnalogY;
			float max = ushort::MaxValue;
			if( analogX == 0 )
				analogX = ( int )( ( ( ( float )analogX / max ) + 0.5f ) * 256 );
			if( analogY == 0 )
				analogY = ( int )( ( ( ( float )analogY / max ) + 0.5f ) * 256 );

			*( ( int* )p ) = ( int )Environment::TickCount;
			*( ( int* )( p + 4 ) ) = ~( int )buttons;
			*( p + 8 ) = ( byte )analogX;
			*( p + 9 ) = ( byte )analogY;
			// 6 bytes of junk
			p += 16;
		}
	}

	return count;
}

//typedef struct SceCtrlLatch {
//	unsigned int 	uiMake;		// keys pressed since last reading
//	unsigned int 	uiBreak;	// keys released since last reading
//	unsigned int 	uiPress;	// bitmask of pressed keys?
//	unsigned int 	uiRelease;	// bitmask of released keys?
//} SceCtrlLatch;

#pragma unmanaged
int sceCtrlPeekLatchN( MemorySystem* memory, int latch_data )
{
	int* ptr = ( int* )memory->Translate( latch_data );
	*ptr			= _makedButtons;
	*( ptr + 1 )	= _breakedButtons;
	*( ptr + 2 )	= _pressedButtons;
	*( ptr + 3 )	= _releasedButtons;
	return 0;
}
#pragma managed

// int sceCtrlPeekLatch(SceCtrlLatch *latch_data); (/ctrl/pspctrl.h:172)
int sceCtrl::sceCtrlPeekLatch( IMemory^ memory, int latch_data )
{
	int* ptr = ( int* )MSI( memory )->Translate( latch_data );
	*ptr			= _makedButtons;
	*( ptr + 1 )	= _breakedButtons;
	*( ptr + 2 )	= _pressedButtons;
	*( ptr + 3 )	= _releasedButtons;

	return 0;
}

#pragma unmanaged
int sceCtrlReadLatchN( MemorySystem* memory, int latch_data )
{
	int* ptr = ( int* )memory->Translate( latch_data );
	*ptr			= _makedButtons;
	*( ptr + 1 )	= _breakedButtons;
	*( ptr + 2 )	= _pressedButtons;
	*( ptr + 3 )	= _releasedButtons;

	_makedButtons = 0;
	_breakedButtons = 0;

	return 0;
}
#pragma managed

// int sceCtrlReadLatch(SceCtrlLatch *latch_data); (/ctrl/pspctrl.h:174)
int sceCtrl::sceCtrlReadLatch( IMemory^ memory, int latch_data )
{
	int* ptr = ( int* )MSI( memory )->Translate( latch_data );
	*ptr			= _makedButtons;
	*( ptr + 1 )	= _breakedButtons;
	*( ptr + 2 )	= _pressedButtons;
	*( ptr + 3 )	= _releasedButtons;

	_makedButtons = 0;
	_breakedButtons = 0;

	return 0;
}