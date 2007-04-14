// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Noxa.Utilities;
using Noxa.Emulation.Psp;
using Noxa.Emulation.Psp.Bios;
using Noxa.Emulation.Psp.Cpu;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE.Modules
{
	class sceRtc : Module
	{
		#region Properties

		public override string Name
		{
			get
			{
				return "sceRtc";
			}
		}

		#endregion

		#region State Management

		public sceRtc( Kernel kernel )
			: base( kernel )
		{
		}

		public override void Start()
		{
		}

		public override void Stop()
		{
		}

		#endregion

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xC41C2853, "sceRtcGetTickResolution" )]
		// SDK location: /rtc/psprtc.h:46
		// SDK declaration: u32 sceRtcGetTickResolution();
		int sceRtcGetTickResolution(){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3F7AD767, "sceRtcGetCurrentTick" )]
		// SDK location: /rtc/psprtc.h:54
		// SDK declaration: int sceRtcGetCurrentTick(u64 *tick);
		int sceRtcGetCurrentTick( int tick ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4CFA57B0, "sceRtcGetCurrentClock" )]
		// SDK location: /rtc/psprtc.h:63
		// SDK declaration: int sceRtcGetCurrentClock(pspTime *time, int tz);
		int sceRtcGetCurrentClock( int time, int tz ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE7C27D1B, "sceRtcGetCurrentClockLocalTime" )]
		// SDK location: /rtc/psprtc.h:71
		// SDK declaration: int sceRtcGetCurrentClockLocalTime(pspTime *time);
		int sceRtcGetCurrentClockLocalTime( int time ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x34885E0D, "sceRtcConvertUtcToLocalTime" )]
		// SDK location: /rtc/psprtc.h:80
		// SDK declaration: int sceRtcConvertUtcToLocalTime(const u64* tickUTC, u64* tickLocal);
		int sceRtcConvertUtcToLocalTime( int tickUTC, int tickLocal ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x779242A2, "sceRtcConvertLocalTimeToUTC" )]
		// SDK location: /rtc/psprtc.h:89
		// SDK declaration: int sceRtcConvertLocalTimeToUTC(const u64* tickLocal, u64* tickUTC);
		int sceRtcConvertLocalTimeToUTC( int tickLocal, int tickUTC ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x42307A17, "sceRtcIsLeapYear" )]
		// SDK location: /rtc/psprtc.h:97
		// SDK declaration: int sceRtcIsLeapYear(int year);
		int sceRtcIsLeapYear( int year ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x05EF322C, "sceRtcGetDaysInMonth" )]
		// SDK location: /rtc/psprtc.h:106
		// SDK declaration: int sceRtcGetDaysInMonth(int year, int month);
		int sceRtcGetDaysInMonth( int year, int month ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x57726BC1, "sceRtcGetDayOfWeek" )]
		// SDK location: /rtc/psprtc.h:116
		// SDK declaration: int sceRtcGetDayOfWeek(int year, int month, int day);
		int sceRtcGetDayOfWeek( int year, int month, int day ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x4B1B5E82, "sceRtcCheckValid" )]
		// SDK location: /rtc/psprtc.h:124
		// SDK declaration: int sceRtcCheckValid(const pspTime* date);
		int sceRtcCheckValid( int date ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x3A807CC8, "sceRtcSetTime_t" )]
		// SDK location: /rtc/psprtc.h:244
		// SDK declaration: int sceRtcSetTime_t(pspTime* date, const time_t time);
		int sceRtcSetTime_t( int date, int time ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x27C4594C, "sceRtcGetTime_t" )]
		// SDK location: /rtc/psprtc.h:245
		// SDK declaration: int sceRtcGetTime_t(const pspTime* date, time_t *time);
		int sceRtcGetTime_t( int date, int time ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF006F264, "sceRtcSetDosTime" )]
		// SDK location: /rtc/psprtc.h:246
		// SDK declaration: int sceRtcSetDosTime(pspTime* date, u32 dosTime);
		int sceRtcSetDosTime( int date, int dosTime ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x36075567, "sceRtcGetDosTime" )]
		// SDK location: /rtc/psprtc.h:247
		// SDK declaration: int sceRtcGetDosTime(pspTime* date, u32 dosTime);
		int sceRtcGetDosTime( int date, int dosTime ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7ACE4C04, "sceRtcSetWin32FileTime" )]
		// SDK location: /rtc/psprtc.h:248
		// SDK declaration: int sceRtcSetWin32FileTime(pspTime* date, u64* win32Time);
		int sceRtcSetWin32FileTime( int date, int win32Time ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCF561893, "sceRtcGetWin32FileTime" )]
		// SDK location: /rtc/psprtc.h:249
		// SDK declaration: int sceRtcGetWin32FileTime(pspTime* date, u64* win32Time);
		int sceRtcGetWin32FileTime( int date, int win32Time ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x7ED29E40, "sceRtcSetTick" )]
		// SDK location: /rtc/psprtc.h:133
		// SDK declaration: int sceRtcSetTick(pspTime* date, const u64* tick);
		int sceRtcSetTick( int date, int tick ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x6FF40ACC, "sceRtcGetTick" )]
		// SDK location: /rtc/psprtc.h:142
		// SDK declaration: int sceRtcGetTick(const pspTime* date, u64 *tick);
		int sceRtcGetTick( int date, int tick ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x011F03C1, "sceRtcGetAccumulativeTime" )]
		// manual add - probably like sceRtcGetTick?
		int sceRtcGetAccumulativeTime( int date, int tick ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x9ED0AE87, "sceRtcCompareTick" )]
		// SDK location: /rtc/psprtc.h:151
		// SDK declaration: int sceRtcCompareTick(const u64* tick1, const u64* tick2);
		int sceRtcCompareTick( int tick1, int tick2 ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x44F45E05, "sceRtcTickAddTicks" )]
		// SDK location: /rtc/psprtc.h:161
		// SDK declaration: int sceRtcTickAddTicks(u64* destTick, const u64* srcTick, u64 numTicks);
		int sceRtcTickAddTicks( int destTick, int srcTick, int numTicks ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26D25A5D, "sceRtcTickAddMicroseconds" )]
		// SDK location: /rtc/psprtc.h:171
		// SDK declaration: int sceRtcTickAddMicroseconds(u64* destTick, const u64* srcTick, u64 numMS);
		int sceRtcTickAddMicroseconds( int destTick, int srcTick, int numMS ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xF2A4AFE5, "sceRtcTickAddSeconds" )]
		// SDK location: /rtc/psprtc.h:181
		// SDK declaration: int sceRtcTickAddSeconds(u64* destTick, const u64* srcTick, u64 numSecs);
		int sceRtcTickAddSeconds( int destTick, int srcTick, int numSecs ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE6605BCA, "sceRtcTickAddMinutes" )]
		// SDK location: /rtc/psprtc.h:191
		// SDK declaration: int sceRtcTickAddMinutes(u64* destTick, const u64* srcTick, u64 numMins);
		int sceRtcTickAddMinutes( int destTick, int srcTick, int numMins ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x26D7A24A, "sceRtcTickAddHours" )]
		// SDK location: /rtc/psprtc.h:201
		// SDK declaration: int sceRtcTickAddHours(u64* destTick, const u64* srcTick, int numHours);
		int sceRtcTickAddHours( int destTick, int srcTick, int numHours ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xE51B4B7A, "sceRtcTickAddDays" )]
		// SDK location: /rtc/psprtc.h:211
		// SDK declaration: int sceRtcTickAddDays(u64* destTick, const u64* srcTick, int numDays);
		int sceRtcTickAddDays( int destTick, int srcTick, int numDays ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xCF3A2CA8, "sceRtcTickAddWeeks" )]
		// SDK location: /rtc/psprtc.h:221
		// SDK declaration: int sceRtcTickAddWeeks(u64* destTick, const u64* srcTick, int numWeeks);
		int sceRtcTickAddWeeks( int destTick, int srcTick, int numWeeks ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDBF74F1B, "sceRtcTickAddMonths" )]
		// SDK location: /rtc/psprtc.h:232
		// SDK declaration: int sceRtcTickAddMonths(u64* destTick, const u64* srcTick, int numMonths);
		int sceRtcTickAddMonths( int destTick, int srcTick, int numMonths ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0x42842C77, "sceRtcTickAddYears" )]
		// SDK location: /rtc/psprtc.h:242
		// SDK declaration: int sceRtcTickAddYears(u64* destTick, const u64* srcTick, int numYears);
		int sceRtcTickAddYears( int destTick, int srcTick, int numYears ){ return Module.NotImplementedReturn; }

		[NotImplemented]
		[Stateless]
		[BiosFunction( 0xDFBC5F16, "sceRtcParseDateTime" )]
		// SDK location: /rtc/psprtc.h:251
		// SDK declaration: int sceRtcParseDateTime(u64 *destTick, const char *dateString);
		int sceRtcParseDateTime( int destTick, int dateString ){ return Module.NotImplementedReturn; }

	}
}

/* GenerateStubsV2: auto-generated - D5F1F444 */