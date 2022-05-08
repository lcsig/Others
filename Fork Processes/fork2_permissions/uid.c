#include <stdio.h>
#include <unistd.h>
 
int main () {
 
  puts("[+] --------------------------------------------------");
  printf("[+] -------------- CHILD  PID : %d ---------------- \n", getpid());
  puts("[+] --------------------------------------------------");

  printf("[*] \t\t UID  \t\t\t GID  \n"
        "[-] Real     \t %d\t Real     \t %d  \n"
        "[-] Effective\t %d\t Effective\t %d  \n",
            getuid (),     getgid (),
            geteuid(),     getegid()
    );

  puts("");
  puts("[+] --------------------------------------------------");
  puts("[+] -------------- Calling seteuid(0) ----------------");
  puts("[+] --------------------------------------------------");

	seteuid(0);
  printf("[*] \t\t UID  \t\t\t GID  \n"
        "[-] Real     \t %d\t Real     \t %d  \n"
        "[-] Effective\t %d\t Effective\t %d  \n",
            getuid (),     getgid (),
            geteuid(),     getegid()
    );
    return 0;

}

