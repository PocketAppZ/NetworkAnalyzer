﻿using System.Net.NetworkInformation;
using static NetworkAnalyzer.Apps.GlobalClasses.DataStore;

namespace NetworkAnalyzer.Apps.LatencyMonitor.Functions
{
    internal class PacketLossHandler
    {
        // Determine the total number of packets lost by checking whether or not the ping requests were successful
        public async Task<int> CalculateTotalPacketsLostAsync(IPStatus status, string targetName, bool initialization)
        {
            int packetsLost = 0;

            if (initialization)
            {
                if (status != IPStatus.Success)
                {
                    packetsLost = 1;
                }
                else
                {
                    packetsLost = 0;
                }
            }
            else
            {
                var lastDataSet = LiveSessionData[targetName].LastOrDefault();

                if (status != IPStatus.Success)
                {
                    packetsLost = lastDataSet.TotalPacketsLost + 1;
                }
                else
                {
                    packetsLost = lastDataSet.TotalPacketsLost;
                }
            }

            return await Task.FromResult(packetsLost);
        }

        // Determine whether the ping test failed and increment the FailedSessionPackets dictionary accordingly
        public async Task<bool> CalculateFailedPingAsync(IPStatus status, string targetName, bool initialization)
        {
            bool pingFailed = false;

            if (initialization)
            {
                if (status == IPStatus.Success)
                {
                    pingFailed = false;
                }
                else
                {
                    pingFailed = true;
                }
            }
            else
            {
                int failedPingCount = FailedSessionPackets[targetName];

                if (status != IPStatus.Success)
                {
                    pingFailed = true;
                    FailedSessionPackets.AddOrUpdate(targetName, failedPingCount, (key, failedPingCount) => failedPingCount + 1);
                }
                else
                {
                    pingFailed = false;
                }
            }

            return await Task.FromResult(pingFailed);
        }
    }
}
