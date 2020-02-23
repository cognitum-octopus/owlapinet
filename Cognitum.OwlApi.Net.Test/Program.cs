/* ============================================================================================================== 
 * This file is part of OWL API for .Net.
 * © 2014 Cognitum, Poland. All rights reserved.  
 * 
 * Licensed under DUAL LICENSE: the Apache 2.0 OR GPLv3
 * Choose the license that is compatible with your
 *
 * License 1:
 * Apache License, Version 2.0 (the "License"); you may not use this file except in compliance  
 * with the License. You may obtain a copy of the License at http: *www.apache.org/licenses/LICENSE-2.0 
 * Unless required by applicable law or agreed to in writing, software distributed under the License is  
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
 * See the License for the specific language governing permissions and limitations under the License. 
 *
 * License 2:
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License v3 as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License v3 for more details.
 *
 * You should have received a copy of the GNU General Public License v3.
 * If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
 *
 * ============================================================================================================== 
 */

/**
 * @author Alessandro Seganti, a.seganti@cognitum.eu, Cognitum Poland
 *         , Date: 18-Aug-2014
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cognitum.OwlApi.Net.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(TryOwlReasoner.CurrentDomain_AssemblyResolve);

            // load HermiT reasoner and show info
            TryOwlReasoner.GetOwlInfo(@"Reasoners\HermiT\");

            // load Pellet reasoner and show info
            TryOwlReasoner.GetOwlInfo(@"Reasoners\Pellet\");

            Examples ex = new Examples();

            ex.shouldLoad();
            Console.ReadLine();
        }
    }

}
