﻿using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using static NetworkAnalyzer.Apps.GlobalClasses.DataStore;

namespace NetworkAnalyzer.Apps.LatencyMonitor.Functions
{
    public class LatencyHandler
    {
        // Calculate the lowest latency by comparing the current latency with the last reported lowest latency
        public async Task<int> CalculateLowestLatencyAsync(IPStatus status, int latency, string targetName, bool initialization)
        {
            int lowestLatency = 0;

            if (initialization)
            {
                lowestLatency = latency;
            }
            else
            {
                var lastDataSet = LiveSessionData[targetName].LastOrDefault();

                if (status == IPStatus.Success && latency <= lastDataSet.LowestLatency)
                {
                    lowestLatency = latency;
                }
                else
                {
                    lowestLatency = lastDataSet.LowestLatency;
                }
            }

            return await Task.FromResult(lowestLatency);
        }

        // Calculate the highest latency by comparing the current latency with the last reported highest latency
        public async Task<int> CalculateHighestLatencyAsync(IPStatus status, int latency, string targetName, bool initialization)
        {
            int highestLatency = 0;

            if (initialization)
            {
                highestLatency = latency;
            }
            else
            {
                var lastDataSet = LiveSessionData[targetName].LastOrDefault();

                if (status == IPStatus.Success && latency >= lastDataSet.HighestLatency)
                {
                    highestLatency = latency;
                }
                else
                {
                    highestLatency = lastDataSet.HighestLatency;
                }
            }

            return await Task.FromResult(highestLatency);
        }

        // Calculate the average latency of all latencies currently stored in the LiveData dictionary
        public async Task<int> CalculateAverageLatencyAsync(IPStatus status, int latency, string targetName, bool initialization)
        {
            int averageLatency = 0;

            if (initialization)
            {
                averageLatency = latency;
            }
            else
            {
                var lastDataSet = LiveSessionData[targetName];

                if (status == IPStatus.Success && latency > 0 && lastDataSet.LastOrDefault().AverageLatency > 0)
                {
                    averageLatency = lastDataSet.LastOrDefault().TotalLatency / lastDataSet.Count;
                }
                else if (status == IPStatus.Success && latency > 0 && lastDataSet.LastOrDefault().AverageLatency == 0)
                {
                    averageLatency = latency;
                }
                else
                {
                    averageLatency = lastDataSet.LastOrDefault().AverageLatency;
                }
            }

            return await Task.FromResult(averageLatency);
        }

        // Calculate the total of all latencies returned from the ping tests for use with calculating the average latency
        public async Task<int> CalculateTotalLatencyAsync(int latency, string targetName, bool initialization)
        {
            int totalLatency = 0;

            if (initialization)
            {
                totalLatency = latency;
            }
            else
            {
                var lastDataSet = LiveSessionData[targetName].LastOrDefault();

                totalLatency = lastDataSet.TotalLatency + latency;
            }

            return await Task.FromResult(totalLatency);
        }
    }
}
