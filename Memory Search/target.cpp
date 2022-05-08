#include <iostream>
#include <Windows.h>
#include <string>
using namespace std;
 
struct Test {
    char Name[20];
    int Age;
} var;
 
void main() {
    std::cout << "Enter The Name :";
    cin >> var.Name;
    cout << "Enter The Age :";
    cin >> var.Age;
 
    cout << " The Structure Pointer " << &var << endl;
    cout << " The Structure Pointer " << &var.Age << endl;
    cout << " The Structure Pointer " << &var.Name << endl;
    cout << " The Process ID :" << GetCurrentProcessId() << endl;
    
    system("pause"); system("pause"); system("pause"); system("pause");
}

