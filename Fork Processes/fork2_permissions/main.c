#include <stdio.h>
#include <unistd.h>


void print_info() {
  printf("[*] \t\t UID  \t\t\t GID  \n"
        "[-] Real     \t %d\t Real     \t %d  \n"
        "[-] Effective\t %d\t Effective\t %d  \n",
            getuid (),     getgid (),
            geteuid(),     getegid()
    );
  return;
}


int main () {

  printf("[+] ----------- MAIN, FATHER PID : %d ------------- \n", getpid());
  print_info();
  printf("--------------------------------\n");

  pid_t pid;
  int status;
  pid = fork();

  seteuid(getuid());
  // printf("[+] ------------ Calling seteuid(getuid()); -------------\n\n\n");

  if (!pid) {
    puts("[+] ############## Child process #####################\n\n");
    execv("./uid.out", NULL);
    return;
  }

  wait(status);
  puts("\n\n[+] ############## End Child process #################\n\n");

  printf("[+] ----------- MAIN, FATHER PID : %d ------------- \n", getpid());
  print_info();
  puts("--------------------------------");

  return 0;
}
