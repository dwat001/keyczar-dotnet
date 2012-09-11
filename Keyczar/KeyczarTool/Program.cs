﻿// Copyright 2012 James Tuley (jay+code@tuley.name)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManyConsole;

namespace KeyczarTool
{
    class Program
    {
        static void Main(string[] args)
        { 
            
            // locate any commands in the assembly (or use an IoC container, or whatever source)
            var commands = new ConsoleCommand[]
                               {
                                   new Create(), 
                                   new AddKey(), 
                                   new PubKey(), 
                                   new Promote(), 
                                   new Demote(), 
                                   new Revoke(), 
                                   new ImportKey(),
                                   new UseKey(), 
                               };

            // run the command for the console input
            ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);

        }
    }
}