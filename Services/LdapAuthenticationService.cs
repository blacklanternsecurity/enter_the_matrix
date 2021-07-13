/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Enter_The_Matrix.Models;
using Microsoft.Extensions.Options;
using System;
using Novell;
using Novell.Directory.Ldap;
using System.Net.NetworkInformation;
using DocumentFormat.OpenXml.Spreadsheet;
using Novell.Directory.Ldap.Controls;
using System.Collections;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Linq;

namespace Enter_The_Matrix
{
    public class LdapAuthenticationService : IAuthenticationService
    {
        private const string DisplayNameAttribute = "DisplayName";
        private const string SAMAccountNameAttribute = "SAMAccountName";

        private readonly LdapConfig config;

        public LdapAuthenticationService(IOptions<LdapConfig> config)
        {
            this.config = config.Value;
        }

        public User Login(string userName, string password)
        {
            User user = new User();


            using (var cn = new Novell.Directory.Ldap.LdapConnection())
            {
                cn.Connect(config.Path, config.Port);

                try
                {
                    cn.Bind(config.UserDomainName + "\\" + userName, password);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Failed login attempt for user " + userName);
                    user = null;
                    return user;

                }

                string filter = "sAMAccountname=" + userName;
                
                string baseStr = "OU=BLS,DC=blacklanternsecurity,DC=com";

                LdapSearchResults result = (LdapSearchResults)cn.Search(baseStr, LdapConnection.ScopeSub, filter, null, false);

                LdapEntry entry = null;
                try
                {
                    entry = result.First();
                }
                catch(LdapException e)
                {
                    Console.WriteLine("Error: " + e.LdapErrorMessage);
                }

                LdapAttributeSet attributeSet = entry.GetAttributeSet();

                user.DisplayName = attributeSet.GetAttribute("displayName").StringValue;
                user.GivenName = attributeSet.GetAttribute("givenName").StringValue;
                user.UserName = userName;

                return user;
            }
        }
    }

    public interface IAuthenticationService
    {
        User Login(string userName, string password);
    }
}
