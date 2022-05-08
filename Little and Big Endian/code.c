#include <stdio.h>
 
int isItLittleEndian() {
    int x = 0x0A0B0C0D;
 
    if (*(char*)(&x) == 0x0D)
        return 1;
    else 
        return 0;
    
}
 
 
int main(int argc, char ** argv) {
 
    if (isItLittleEndian() == 1)
        printf("[*] Little endian ...\n\n");
    else
        printf("[*] Big endian ...\n\n");
 
 
    int x = 0x0A0B0C0D;
    char *y = (void*) &x;
    printf("[!] X Value     : 0x%08x \n", x);
    printf("[!] X Location  : 0x%x   \n\n", &x);
 
    for (int i=0; i < sizeof(x); i++)
        printf("[-] Value 0x%x, At 0x%x ... \n", *((char*)(&x) + i), ((char*)(&x) + i) );
 
    return 0;
 
}

