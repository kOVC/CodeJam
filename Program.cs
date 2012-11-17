/*
    Program.cs is a sample entry point file to show how to use the Input/OutputFile classes.
	It assumes the input was named a.in and it will output to a.txt
	
    Copyright (C) 2012  Mohamed Abu Marsa a.k.a. (VC, kOVC) (vc@korganization.org)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
	The GNU General Public License copy can be found in the COPYRIGHT file 
	at the root of the project directory structure.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CJ {
    class Program {
        static void Main(string[] args) {
            InputFile input = new InputFile("a.in");
            OutputFile ouptut = new OutputFile("a.txt");
        }
    }
}
