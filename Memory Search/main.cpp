#include <iostream>
#include <Windows.h>
#include <vector>
using namespace std;
 
struct Test {
    char Name[20];
    int Age;
} var;
 
BOOL GetAddressOfData(LPVOID X[], int pid, const char *data, size_t len);
void main() {
    int ProcId = 0;
    cout << "Enter the process ID :";
    cin >> ProcId;
    cout << "Enter the name from the target structure :";
    cin >> var.Name;
 
    LPVOID x[20] = {};
    if (GetAddressOfData(x, ProcId, var.Name, sizeof(var.Name))) {
        HANDLE hProc = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, FALSE, ProcId);
        for (int i = 0; i < 20; i++) {
            if (x[i]) {
                cout << "Found @ " << x[i] << endl;
                cout << "   Result Of Reading :" << ReadProcessMemory(hProc, x[i], &var, sizeof(var), NULL) << endl;
                cout << "       The Age :" << var.Age << endl;
            }
        }
        CloseHandle(hProc);
    }
    else cout << "Not Found\n";
    system("pause");
}
 
 
BOOL GetAddressOfData(LPVOID X[] , int pid, const char *data, size_t len) {
    int ii = 0;
    HANDLE hProc = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, FALSE, pid);
    if (hProc) {
        SYSTEM_INFO si;
        GetSystemInfo(&si);
    
        vector<char> Buff;
        char* p = (char*)si.lpMinimumApplicationAddress;
        MEMORY_BASIC_INFORMATION info ;
        //
        while (p < si.lpMaximumApplicationAddress) {
            try {
                if (VirtualQueryEx(hProc, p, &info, sizeof(info))) {
                    if (info.State = MEM_COMMIT) {
                        p = (char*)info.BaseAddress;         
                        Buff.resize(info.RegionSize);
                        SIZE_T bytesRead;
                        if (ReadProcessMemory(hProc, p, &Buff[0], info.RegionSize, &bytesRead)) {
                            for (size_t i = 0; i < (bytesRead - len); ++i)
                                if (memcmp(data, &Buff[i], len) == 0) {
                                    X[ii] = (p + i);
                                    ii++;
                                }
                        }
                    }
                } 
             }  catch (...) {}
             p += info.RegionSize;
        }
        
    }
    CloseHandle(hProc);
    if (X[0]) return 1;
    else return 0;
}

