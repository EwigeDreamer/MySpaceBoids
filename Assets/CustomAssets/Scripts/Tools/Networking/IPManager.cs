using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public static class IPManager
{
    public enum ADDRESSFAM
    {
        IPv4, IPv6
    }

    public static void GetAllIPs(List<string> list, ADDRESSFAM Addfam, bool includeDetails)
    {
        if (list == null) return;
        list.Clear();

        //Return null if ADDRESSFAM is Ipv6 but Os does not support it
        if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6) return;

        foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            bool isCandidate = (item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2);

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            // as of MacOS (10.13) and iOS (12.1), OperationalStatus seems to be always "Unknown".
            isCandidate = isCandidate && item.OperationalStatus == OperationalStatus.Up;
#endif
            if (isCandidate)
#endif 
            {
                foreach (var info in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (Addfam == ADDRESSFAM.IPv4)
                    {
                        if (info.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            string s = info.Address.ToString();
                            if (includeDetails)
                            {
                                s += "  " + item.Description.PadLeft(6) + item.NetworkInterfaceType.ToString().PadLeft(10);
                            }
                            list.Add(s);
                        }
                    }

                    //IPv6
                    else if (Addfam == ADDRESSFAM.IPv6)
                    {
                        if (info.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            list.Add(info.Address.ToString());
                        }
                    }
                }
            }
        }
    }
}
