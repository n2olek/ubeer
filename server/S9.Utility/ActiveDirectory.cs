using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace S9.Utility
{
    public class ActiveDirectory
    {
        private string m_ADPath = string.Empty;
        private string m_ADUser = string.Empty;
        private string m_ADPassword = string.Empty;

        public ActiveDirectory(string adPath, string adUsername, string adPassword)
        {
            m_ADPath = adPath.Trim();
            m_ADUser = adUsername.Trim();
            m_ADPassword = adPassword.Trim();
        }

        // The method gets all users from AD
        // and returns distinct office as list of groups
        // so use it wisely to prevent lower performance
        public List<ADUserGroup> GetGroups()
        {
            var userList = GetUsers();

            var result = (
                            from u in userList
                            group u by new { u.Office } into g
                            select new ADUserGroup
                              {
                                  Name = g.Key.Office,
                                  AdUsers = GetGroupUserList(g.Key.Office)
                              }
                        );

            return result.ToList();
        }

        // heavily get all users from AD (5000 max)
        // use it wisely or it will turn down performance
        public List<ADUser> GetUsers()
        {
            var users = new List<ADUser>();

            var dirEntry = new DirectoryEntry(m_ADPath) { Username = m_ADUser, Password = m_ADPassword };
            var dirSearch = new DirectorySearcher
                          {
                              Filter = @"(&(ObjectCategory=Person)(ObjectClass=user))",
                              SearchRoot = dirEntry,
                              SearchScope = SearchScope.Subtree,
                              SizeLimit = 5000,
                              PageSize = 5000
                          };

            try
            {
                var result = dirSearch.FindAll();

                foreach (SearchResult searchResult in result)
                {
                    var displayName = string.Empty;
                    var firstName = string.Empty;
                    var lastname = string.Empty;
                    var office = string.Empty;
                    var email = string.Empty;

                    displayName = searchResult.Properties.Contains("name")? searchResult.Properties["name"][0].ToString(): string.Empty;
                    firstName = searchResult.Properties.Contains("givenname") ? searchResult.Properties["givenname"][0].ToString() : string.Empty;
                    lastname = searchResult.Properties.Contains("sn") ? searchResult.Properties["sn"][0].ToString() : string.Empty;
                    email = searchResult.Properties.Contains("mail") ? searchResult.Properties["mail"][0].ToString() : string.Empty;

                    if(
                        !string.IsNullOrWhiteSpace(email) &&
                        !string.IsNullOrWhiteSpace(office)
                      )
                    {
                        var user = new ADUser
                        {
                            DisplayName = displayName,
                            FirstName = firstName,
                            LastName = lastname,
                            Office = office,
                            Email = email
                        };

                        users.Add(user);
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        
        // admin credentials is required
        // to get a user information
        public ADUser GetUser(string username)
        {
            var dirEntry = new DirectoryEntry(m_ADPath) { Username = m_ADUser, Password = m_ADPassword };
            var extractName = username.Split('@');

            var dirSearch = new DirectorySearcher
                          {
                              SearchRoot = dirEntry,
                              Filter = "(&(ObjectClass=user)(sAMAccountName=" + extractName[0] + "))",
                              SearchScope = SearchScope.Subtree,
                              PageSize = 1000
                          };


            try
            {
                var result = dirSearch.FindOne();

                if (null != result)
                {
                    dirEntry = new DirectoryEntry(result.Path, m_ADUser, m_ADPassword, AuthenticationTypes.Secure);

                    var user = new ADUser
                    {
                        DisplayName = dirEntry.Properties["name"][0].ToString(),
                        FirstName = dirEntry.Properties["givenname"][0].ToString(),
                        LastName = dirEntry.Properties["sn"][0].ToString(),
                        //Office = ode.Properties["physicalDeliveryOfficeName"][0].ToString(),
                        Email = dirEntry.Properties["mail"][0].ToString(),
                        EmpID = dirEntry.Properties["pager"][0].ToString()
                    };

                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                
                return null;
            }
        }
        

        public ADUser GetUser(string userName, string password)
        {
            //LogUtil.InfoInternal("AD: prepare directory entry object");

            var dirEntry = new DirectoryEntry(m_ADPath) { Username = userName, Password = password };
            var extractName = userName.Split('@');

            //LogUtil.InfoInternal("AD: prepare directory searcher object");  
            var dirSearch = new DirectorySearcher
                          {
                              SearchRoot = dirEntry,
                              Filter = "(&(ObjectClass=user)(sAMAccountName=" + extractName[0] + "))",
                              SearchScope = SearchScope.Subtree,
                              PageSize = 1000
                          };

            try
            {
                var result = dirSearch.FindOne();
                
                if (null != result)
                {
                    //LogUtil.InfoInternal("AD: found entry"); 
                
                    dirEntry = new DirectoryEntry(result.Path, userName, password, AuthenticationTypes.Secure);
                    
                    /*/
                    foreach (DirectoryEntry de in ode.Properties)
                    {
                        string s = de.Path;
                    }
                    /*/

                    var user = new ADUser
                    {
                        //EmpID = dirEntry.Properties["pager"][0].ToString(),
                        //Email = dirEntry.Properties["mail"][0].ToString(),
                        //DisplayName = dirEntry.Properties["name"][0].ToString(),
                        DisplayName = dirEntry.Properties["displayname"][0].ToString(),
                        //FirstName = dirEntry.Properties["givenname"][0].ToString(),
                        LastName = dirEntry.Properties["sn"][0].ToString(),
                    };

                    return user;                    
                }

                //LogUtil.Info("AD: the entry is null"); 
                return null;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return null;
        }


        // admin credentials is required
        // to get a user information
        public ADUser GetUserByEmpID(string empID)
        {
            var dirEntry = new DirectoryEntry(m_ADPath) { Username = m_ADUser, Password = m_ADPassword };

            var dirSearch = new DirectorySearcher
            {
                SearchRoot = dirEntry,
                Filter = "(&(ObjectClass=user)(pager=" + empID + "))",
                SearchScope = SearchScope.Subtree,
                PageSize = 1000
            };


            try
            {
                var result = dirSearch.FindOne();

                if (null != result)
                {
                    dirEntry = new DirectoryEntry(result.Path, m_ADUser, m_ADPassword, AuthenticationTypes.Secure);

                    var user = new ADUser
                    {
                        EmpID = dirEntry.Properties["pager"][0].ToString(),
                        Email = dirEntry.Properties["mail"][0].ToString(),
                        DisplayName = dirEntry.Properties["name"][0].ToString(),                        
                        FirstName = dirEntry.Properties["givenname"][0].ToString(),
                        LastName = dirEntry.Properties["sn"][0].ToString(),
                    };

                    return user;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }



        /// <summary>
        ///     ValidateLDAPUser - To validate if the user/password is valid in LDAP
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password (plain text)</param>
        /// <param name="LDAPType">platform either "sun" or "windows"</param>
        /// <returns>true or false</returns> 
        public bool ValidateLDAPUser(string userName, string password, LDAP_TYPE LDAPType)
        {

            DirectoryEntry dirEntry = new DirectoryEntry(m_ADPath, userName, password);

            if (LDAPType == LDAP_TYPE.SUN)
            {
                DirectorySearcher dirSearch = new DirectorySearcher(dirEntry);
                SearchResultCollection searchResult;
                //SearchResult oResult;

                dirEntry.AuthenticationType = AuthenticationTypes.FastBind;
                dirEntry.Username = "uid=" + userName + ",ou=people,o=ktb.co.th,o=kcs";
                dirEntry.Password = password;

                dirSearch.Filter = "(&(objectclass=*)(uid=" + userName + "))";
                try
                {
                    searchResult = dirSearch.FindAll();

                    if (searchResult.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        // Update LDAP Profile //
                        /*/
                        foreach (SearchResult oResult in oResults)
                        {                            
                            strThaiTitle = oResult.Properties["thaipersonaltitle"][0].ToString();
                            strThaiFirstname = oResult.Properties["thaifirstname"][0].ToString();
                            strThaiLastname = oResult.Properties["thailastname"][0].ToString();
                            strEngTitle = oResult.Properties["personaltitle"][0].ToString();
                            strEngFirstname = oResult.Properties["givenname"][0].ToString();
                            strEngLastname = oResult.Properties["sn"][0].ToString();
                            strEmail = oResult.Properties["mail"][0].ToString();                            
                        }
                        /*/


                        return true;
                    }
                }
                catch
                {
                    return false;
                }

            }
            else if (LDAPType == LDAP_TYPE.WINDOWS)
            {
                dirEntry.AuthenticationType = AuthenticationTypes.Secure;

                DirectorySearcher direct_search = new DirectorySearcher();
                direct_search.SearchRoot = dirEntry;
                direct_search.Filter = "(&(ObjectClass=user)(SAMAccountName=" + userName + "))";
                try
                {
                    SearchResultCollection result_col = direct_search.FindAll();
                    return result_col.Count > 0;
                }
                catch
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }


        public string Validate(string username, string password)
        {
            var user = GetUser(username, password);
            
            if( user != null )
                return user.EmpID;
            else
                return "";
        }
        

        // get all users belong to a group (office)
        public List<ADUser> GetGroupUserList(string groupName)
        {
            var list = new List<ADUser>();
            var dirEntry = new DirectoryEntry(m_ADPath) { Username = m_ADUser, Password = m_ADPassword };
            var dirSearch = new DirectorySearcher
                          {
                              Filter =
                                  @"(&(ObjectCategory=Person)(ObjectClass=user)(physicalDeliveryOfficeName=" +
                                  groupName + @"))",
                              SearchRoot = dirEntry,
                              SearchScope = SearchScope.Subtree,
                              SizeLimit = 5000,
                              PageSize = 5000
                          };

            dirSearch.PropertiesToLoad.Add("displayName"); // Full Thai name
            dirSearch.PropertiesToLoad.Add("givenName"); // First name in English
            dirSearch.PropertiesToLoad.Add("physicalDeliveryOfficeName"); // Office
            dirSearch.PropertiesToLoad.Add("sn"); // Last name in English
            dirSearch.PropertiesToLoad.Add("userPrincipalName"); // email

            try
            {
                var result = dirSearch.FindAll();

                foreach (SearchResult i in result)
                {
                    var displayName = string.Empty;
                    var firstName = string.Empty;
                    var lastname = string.Empty;
                    var office = string.Empty;
                    var email = string.Empty;

                    if (i.Properties.Contains("displayName"))
                        displayName = i.Properties["displayName"][0].ToString();

                    if (i.Properties.Contains("givenName"))
                        firstName = i.Properties["givenName"][0].ToString();

                    if (i.Properties.Contains("sn"))
                        lastname = i.Properties["sn"][0].ToString();

                    if (i.Properties.Contains("physicalDeliveryOfficeName"))
                        office = i.Properties["physicalDeliveryOfficeName"][0].ToString();

                    if (i.Properties.Contains("userPrincipalName"))
                        email = i.Properties["userPrincipalName"][0].ToString();

                    if (!string.IsNullOrWhiteSpace(email) &&
                        !string.IsNullOrWhiteSpace(office))
                    {
                        var user = new ADUser
                        {
                            DisplayName = displayName,
                            FirstName = firstName,
                            LastName = lastname,
                            Office = office,
                            Email = email
                        };

                        list.Add(user);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }


    public class ADUser
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmpID { get; set; }

        // fields for ad user
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Office { get; set; }

        public virtual ADUserGroup AdUserGroup { get; set; }
    }

    public class ADEmpDetail
    {
        public string EmpID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string EFNAME { get; set; }
        public string ELName { get; set; }
        public string Email { get; set; }
    };

    public class ADUserGroup
    {
        public string Name { get; set; }
        public ICollection<ADUser> AdUsers { get; set; }

        public ADUserGroup()
        {
            AdUsers = new HashSet<ADUser>();
        }
    }

    public enum LDAP_TYPE
    {
        WINDOWS = 1,
        SUN = 2,
    }
}
