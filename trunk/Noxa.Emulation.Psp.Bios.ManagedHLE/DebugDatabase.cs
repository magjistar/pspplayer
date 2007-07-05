// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Noxa.Emulation.Psp.Cpu;
using Noxa.Emulation.Psp.Debugging.DebugData;
using Noxa.Emulation.Psp.Debugging.DebugModel;

namespace Noxa.Emulation.Psp.Bios.ManagedHLE
{
	class DebugDatabase : IDebugDatabase
	{
		private List<Method> _methods;
		private List<Variable> _variables;
		private bool _updating;
		private Symbol[] _symbols;

		public DebugDatabase()
		{
			_methods = new List<Method>( 1024 );
			_variables = new List<Variable>( 1024 );
		}

		public void Clear()
		{
			_methods.Clear();
			_variables.Clear();
			_symbols = null;
		}

		public void BeginUpdate()
		{
			_updating = true;
			_symbols = null;
		}

		public void EndUpdate()
		{
			// Symbols contains all the symbols, sorted by address
			// This way, we can do a binary search later on
			// We check for overlap, although there's nothing we can do about it but cry
			_symbols = new Symbol[ _methods.Count + _variables.Count ];
			for( int n = 0; n < _methods.Count; n++ )
				_symbols[ n ] = _methods[ n ];
			for( int n = 0; n < _variables.Count; n++ )
				_symbols[ _methods.Count + n ] = _variables[ n ];
			Array.Sort<Symbol>( _symbols, delegate( Symbol x, Symbol y )
			{
				return x.Address.CompareTo( y.Address );
			} );
			_updating = false;
		}

		public void AddSymbol( Symbol symbol )
		{
			Debug.Assert( _updating == true );
			if( symbol is Variable )
				_variables.Add( ( Variable )symbol );
			else
				_methods.Add( ( Method )symbol );
		}

		public Symbol FindSymbol( uint address )
		{
			Debug.Assert( _updating == false );
			int first = 0;
			int last = _symbols.Length - 1;
			while( first <= last )
			{
				int middle = ( first + last ) / 2;
				Symbol symbol = _symbols[ middle ];
				if( symbol.Address < address )
					first = middle + 1;
				else if( symbol.Address + symbol.Length > address )
					last = middle - 1;
				else
					return symbol;
			}
			return null;
		}

		#region Methods

		public Method[] GetMethods()
		{
			return _methods.ToArray();
		}

		public Method[] GetMethods( MethodType methodType )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		#endregion

		#region Variables

		public Variable[] GetVariables()
		{
			return _variables.ToArray();
		}

		#endregion
	}
}