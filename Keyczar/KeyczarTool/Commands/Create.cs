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
using Keyczar.Unofficial;
using ManyConsole;
using NDesk.Options;
using Keyczar;

namespace KeyczarTool
{
    internal class Create : ConsoleCommand

    {
        private string _location;
        private string _pupose;
        private string _name;
        private bool _asymm;
        private string _asymmAlg;
        private bool _unofficial;

        public Create()
        {
            this.IsCommand("create", Localized.Create);
            this.HasRequiredOption("l|location=", Localized.Location, v => { _location = v; });
            this.HasRequiredOption("o|purpose=", Localized.Purpose, v => { _pupose = v; });
            this.HasOption("n|name=", Localized.Name, v => { _name = v; });
            this.HasOption("a|asymmetric:", Localized.Asymmetric, v =>
                                                                      {
                                                                          _asymm = true;
                                                                          _asymmAlg = v;
                                                                      });
            this.HasOption("u|unofficial:", Localized.UnofficialCreate, v =>
                                                                            {
                                                                                _unofficial = true;
                                                                            });
            this.SkipsCommandSummaryBeforeRunning();
        }

        public override int Run(string[] remainingArguments)
        {
            KeyPurpose purpose = _pupose == "sign" ? KeyPurpose.SignAndVerify : KeyPurpose.DecryptAndEncrypt;

            KeyType type = PickKeyType(purpose);


            var meta = new KeyMetadata()
                           {
                               Name = _name,
                               Purpose = purpose,
                               KeyType = type,
                           };
            using (var keySet = new MutableKeySet(meta))
            {
                if (keySet.Save(new KeySetWriter(_location)))
                {
                    Console.WriteLine(Localized.MsgCreatedKeySet);
                    return 0;
                }
            }
            Console.WriteLine("{0} {1}.", Localized.MsgExistingKeySet, _location);

            return -1;
        }

        private KeyType PickKeyType(KeyPurpose purpose)
        {
            KeyType type;
            if (_asymm)
            {
                if (purpose == KeyPurpose.DecryptAndEncrypt)
                {
                    type = KeyType.RsaPriv;
                }
                else
                {
                    if (_unofficial)
                    {
                        type = UnofficialKeyType.RSAPrivSign;
                    }
                    else if (_asymmAlg == "rsa")
                    {
                        type = KeyType.RsaPriv;
                    }
                    else
                    {
                        type = KeyType.DsaPriv;
                    }
                }
            }
            else if (purpose == KeyPurpose.DecryptAndEncrypt)
            {
                if (_unofficial)
                {
                    type = UnofficialKeyType.AesAead;
                }
                else
                {
                    type = KeyType.Aes;
                }
            }
            else
            {
                type = KeyType.HmacSha1;
            }
            return type;
        }
    }
}