using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace Test
{
    class Program
    {
        static void Main(string[] args) {
            Console.WriteLine("Enter The Process ID :");
            int ProcID = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter The String :");
            string TarStr = Console.ReadLine();
            IntPtr Result = (IntPtr)GetAddressOfData(ProcID, TarStr, (uint)20);
            if (Result != IntPtr.Zero)
                Console.WriteLine("Found @ {0}", Result);
            else Console.WriteLine("Not Found");
            Console.ReadKey();
        }

        const int PROCESS_VM_READ = 0x10;
        const int PROCESS_QUERY_INFORMATION = 0x400;
        const int MEM_COMMIT = 0x1000;
        unsafe static uint GetAddressOfData(int pid, string data, uint len) {
             IntPtr hProc = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, pid);
             if (hProc != null)
             {
                SYSTEM_INFO sys_info = new SYSTEM_INFO();
                GetSystemInfo(out sys_info);

                MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
                byte* p = (byte*)sys_info.minimumApplicationAddress;
                byte[] xx = new byte[20];

                while ((Int64)p < (Int64)sys_info.maximumApplicationAddress) {
                    if (VirtualQueryEx(hProc, (IntPtr)p, out mem_basic_info, 28) != 0) {
                        if (mem_basic_info.State == MEM_COMMIT)
                        {
                         p = (byte*)mem_basic_info.BaseAddress;
                        byte[] Buff = new byte[(uint)mem_basic_info.RegionSize];

                        int bytesRead = 0;
                        bool success = ReadProcessMemory((IntPtr)hProc, (IntPtr)p, Buff, mem_basic_info.RegionSize, out bytesRead);
                        if (success) {
                            for (uint i = 0; i < (bytesRead - len); ++i) {
                                Array.Copy(Buff , i , xx,0, len);
                                if (memcmp(System.Text.Encoding.Default.GetBytes(data), xx, len) == 0) 
                                    return (uint)(p + i);        
                            }
                        }
                        }
                     
                     
                    }
                    p += (uint)mem_basic_info.RegionSize;
                }
            }
            return 0;
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int processAccess, bool bInheritHandle, int processId);

        [DllImport("msvcrt", CallingConvention=CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, uint count);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory( IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer,  int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern void GetSystemInfo(out SYSTEM_INFO Info);
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO {
             public ushort processorArchitecture;
             ushort reserved;
             public uint pageSize;
             public IntPtr minimumApplicationAddress;
             public IntPtr maximumApplicationAddress;
             public IntPtr activeProcessorMask;
             public uint numberOfProcessors;
             public uint processorType;
             public uint allocationGranularity;
             public ushort processorLevel;
             public ushort processorRevision;
        }

    

        [DllImport("kernel32", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
        public struct MEMORY_BASIC_INFORMATION {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;   
            public int State;   
            public int Protect; 
            public int lType;
        }


    }
}

