// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Noxa.Emulation.Psp.Debugging;
using Noxa.Emulation.Psp.Games;
using Noxa.Emulation.Psp.Bios;

namespace Noxa.Emulation.Psp.Cpu
{
	/// <summary>
	/// Delegate used to indicate that a callback marshalling operation has completed.
	/// </summary>
	/// <param name="tcsId">The thread context storage ID that the callback was performed on.</param>
	/// <param name="state">User-passed state argument.</param>
	/// <param name="result">The value of $v0 (probably the result).</param>
	public delegate void MarshalCompleteDelegate( int tcsId, int state, int result );

	/// <summary>
	/// A CPU.
	/// </summary>
	public interface ICpu : IComponentInstance
	{
		/// <summary>
		/// The capability definition instance.
		/// </summary>
		ICpuCapabilities Capabilities
		{
			get;
		}

		/// <summary>
		/// The statistics reporter.
		/// </summary>
		ICpuStatistics Statistics
		{
			get;
		}

		/// <summary>
		/// A list of cores in the CPU.
		/// </summary>
		ICpuCore[] Cores
		{
			get;
		}

		/// <summary>
		/// Get a <see cref="ICpuCore"/> by ordinal.
		/// </summary>
		/// <param name="core">The ordinal of the core to retrieve.</param>
		/// <returns>The <see cref="ICpuCore"/> with the given ordinal.</returns>
		ICpuCore this[ int core ]
		{
			get;
		}

		/// <summary>
		/// The audio/video decoder, if supported.
		/// </summary>
		IAvcDecoder Avc
		{
			get;
		}

		/// <summary>
		/// The memory system.
		/// </summary>
		IMemory Memory
		{
			get;
		}

		/// <summary>
		/// A pointer to the native CPU interface.
		/// </summary>
		IntPtr NativeInterface
		{
			get;
		}

		#region Debugging

		/// <summary>
		/// <c>true</c> if debugging is enabled.
		/// </summary>
		bool DebuggingEnabled
		{
			get;
		}

		/// <summary>
		/// The current debugger instance, if enabled.
		/// </summary>
		IDebugger Debugger
		{
			get;
		}

		/// <summary>
		/// The CPU debug inspection instance.
		/// </summary>
		ICpuHook DebugHook
		{
			get;
		}

		/// <summary>
		/// Enable debugging on the CPU.
		/// </summary>
		/// <param name="debugger">The debugger instance to attach to.</param>
		void EnableDebugging( IDebugger debugger );

		#endregion

		#region Syscalls / Exports

		/// <summary>
		/// Register a syscall.
		/// </summary>
		/// <param name="nid">The NID of the syscall to register.</param>
		/// <returns>The syscall ID used to call the given <paramref name="nid"/>.</returns>
		uint RegisterSyscall( uint nid );

		/// <summary>
		/// Register user module exports.
		/// </summary>
		/// <param name="module">The user module containing the exports to register.</param>
		void RegisterUserExports( BiosModule module );

		/// <summary>
		/// Lookup the address of a user export by NID.
		/// </summary>
		/// <param name="nid">The NID to look up.</param>
		/// <returns>The address of the export with the given NID or <c>0</c> if it was not found.</returns>
		uint LookupUserExport( uint nid );

		#endregion

		#region Interrupts

		/// <summary>
		/// The current interrupts masking flag.
		/// </summary>
		uint InterruptsMask
		{
			get;
			set;
		}

		/// <summary>
		/// Register an interrupt handler.
		/// </summary>
		/// <param name="interruptNumber">Interrupt number (0-67).</param>
		/// <param name="slot">Slot on the interrupt line (0-15).</param>
		/// <param name="address">Address of the user callback code.</param>
		/// <param name="argument">Argument to pass to the user callback code.</param>
		void RegisterInterruptHandler( int interruptNumber, int slot, uint address, uint argument );

		/// <summary>
		/// Unregister an interrupt handler.
		/// </summary>
		/// <param name="interruptNumber">Interrupt number (0-67).</param>
		/// <param name="slot">Slot on the interrupt line (0-15).</param>
		void UnregisterInterruptHandler( int interruptNumber, int slot );

		/// <summary>
		/// Set an interrupt as pending.
		/// </summary>
		/// <param name="interruptNumber">Interrupt number (0-67).</param>
		void SetPendingInterrupt( int interruptNumber );

		#endregion

		#region Threading

		/// <summary>
		/// Allocate thread context storage.
		/// </summary>
		/// <param name="pc">The PC to use.</param>
		/// <param name="registers">The 32 general registers to set initially.</param>
		/// <returns>The ID of the newly allocated thread context storage (<c>tcsId</c>).</returns>
		int AllocateContextStorage( uint pc, uint[] registers );

		/// <summary>
		/// Release a thread context storage.
		/// </summary>
		/// <param name="tcsId">The ID of the thread context storage to release.</param>
		void ReleaseContextStorage( int tcsId );

		/// <summary>
		/// Switch thread contexts.
		/// </summary>
		/// <param name="newTcsId">The thread context storage ID to switch to.</param>
		void SwitchContext( int newTcsId );

		/// <summary>
		/// Suspend the current thread and execute the given callback, resuming at the prior address.
		/// </summary>
		/// <param name="tcsId">The thread context storage ID to run the callback on.</param>
		/// <param name="callbackAddress">The user address of the callback code.</param>
		/// <param name="callbackArguments">A list of arguments to pass to the callback (placed in $a0+).</param>
		/// <param name="resultCallback">Caller-defined handler to execute once the callback has completed.</param>
		/// <param name="state">Caller-defined state to be passed to the handler.</param>
		void MarshalCallback( int tcsId, int callbackAddress, int[] callbackArguments, MarshalCompleteDelegate resultCallback, int state );

		#endregion

		/// <summary>
		/// Resume execution.
		/// </summary>
		void Resume();

		/// <summary>
		/// Break execution.
		/// </summary>
		void Break();

		/// <summary>
		/// Setup the CPU with the given parameters.
		/// </summary>
		/// <param name="game">The current <see cref="GameInformation"/>.</param>
		/// <param name="bootStream">The boot stream of the current game.</param>
		void SetupGame( GameInformation game, Stream bootStream );

		/// <summary>
		/// Execute a block of instructions.
		/// </summary>
		/// <returns>The number of instructions executed.</returns>
		int ExecuteBlock();

		/// <summary>
		/// Stop the CPU.
		/// </summary>
		void Stop();

		/// <summary>
		/// Print the statistics to the console.
		/// </summary>
		void PrintStatistics();
	}
}
