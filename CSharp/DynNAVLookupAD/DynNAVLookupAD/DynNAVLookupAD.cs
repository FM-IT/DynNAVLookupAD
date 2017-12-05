using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;

namespace DynNAVLookupAD
{
    public class DynNAVLookupAD
    {
        public DynNAVLookupAD()
        { }

        GroupPrincipal group;
        List<Principal> users;
        List<WinLogon> myWinLogon;
        int Counter;
        PrincipalContext ctx;

        public int getMembersOfGroupCount(string FromDomainName, string FromGroupName)
        {
            Counter = 0;
            ctx = new PrincipalContext(ContextType.Domain, FromDomainName);
            group = GroupPrincipal.FindByIdentity(ctx, FromGroupName);

            // iterate over its members
            if (group != null)
            {
                myWinLogon = new List<WinLogon>();
                users = new List<Principal>();
                foreach (Principal p in group.Members)
                {
                    if (p is UserPrincipal)
                    {
                        users.Add(p);
                        myWinLogon.Add(new WinLogon(p.Sid.ToString(), p.SamAccountName, p.Name, p.Description, p.DisplayName, p.DistinguishedName, p.Guid.ToString(), p.UserPrincipalName));
                        Counter += 1;
                    }
                }
            }
            return Counter;
        }
        public WinLogon getMembersOfGroupIndex(int i)
        {
            Principal p = users.ElementAt(i);
            //users.IndexOf(p, i);
            WinLogon MyWinLogon = new WinLogon(p.Sid.ToString(), p.SamAccountName, p.Name, p.Description, p.DisplayName, p.DistinguishedName, p.Guid.ToString(), p.UserPrincipalName);
            return MyWinLogon;
        }
        public WinLogon getMembersOfListIndex(int i)
        {
            WinLogon WL;
            WL = myWinLogon.ElementAt(i);
            return WL;
        }

        public IEnumerator<WinLogon> GetEnumerator()
        {
            foreach (var WL in myWinLogon)
                yield return WL;
        }
    }

    // Simple business object.
    public class WinLogon
    {
        string _LogonSID;
        string _WindowsUserName;
        string _FullName;
        string _Description;
        string _DisplayName;
        string _DistinguishedName;
        string _MyGuid;
        string _UserPrincipalName;

        public WinLogon(string NewSID, string NewWindowsUserName, string NewFullName, string NewDescription, string NewDisplayName, string NewDistinguishedName, string NewMyGuid, string NewUserPrincipalName)
        {
            _LogonSID = NewSID;
            _WindowsUserName = NewWindowsUserName;
            _FullName = NewFullName;
            _Description = NewDescription;
            _DisplayName = NewDisplayName;
            _DistinguishedName = NewDistinguishedName;
            _MyGuid = NewMyGuid;
            _UserPrincipalName = NewUserPrincipalName;
        }

        public string LogonSID
        {
            get { return _LogonSID; }
            set { _LogonSID = value; }
        }
        public string WindowsUserName
        {
            get { return _WindowsUserName; }
            set { _WindowsUserName = value; }
        }
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }
        public string DistinguishedName
        {
            get { return _DistinguishedName; }
            set { _DistinguishedName = value; }
        }
        public string MyGuid
        {
            get { return _MyGuid; }
            set { _MyGuid = value; }
        }
        public string UserPrincipalName
        {
            get { return _UserPrincipalName; }
            set { _UserPrincipalName = value; }
        }
    }


    // Collection of WinLogon objects. This class
    // implements IEnumerable so that it can be used
    // with ForEach syntax.
    public class WinLogonColl : IEnumerable
    {
        private WinLogon[] _WinLogonColl;
        public WinLogonColl(WinLogon[] pArray)
        {
            _WinLogonColl = new WinLogon[pArray.Length];

            for (int i = 0; i < pArray.Length; i++)
            {
                _WinLogonColl[i] = pArray[i];
            }
        }

        // Implementation for the GetEnumerator method.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public WinLogonCollEnum GetEnumerator()
        {
            return new WinLogonCollEnum(_WinLogonColl);
        }
    }

    // When you implement IEnumerable, you must also implement IEnumerator.
    public class WinLogonCollEnum : IEnumerator
    {
        public WinLogon[] _WinLogonColl;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public WinLogonCollEnum(WinLogon[] list)
        {
            _WinLogonColl = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _WinLogonColl.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public WinLogon Current
        {
            get
            {
                try
                {
                    return _WinLogonColl[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
