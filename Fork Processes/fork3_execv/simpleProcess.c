#include <stdio.h>
#include <unistd.h>


void print_info() {
  printf("[*] \t\t UID  \t\t\t GID  \n"
        "[-] Real     \t %d\t Real     \t %d  \n"
        "[-] Effective\t %d\t Effective\t %d  \n\n",
            getuid (),     getgid (),
            geteuid(),     getegid()
    );
}

// Try again but with 'sudo chown root simpleProcess.out && sudo chmod u+s simpleProcess.out'
int main () {
  printf("[+] ----------- MAIN PID : %d ------------- \n", getpid());
  print_info();

  pid_t pid;
  int status;
  pid = fork();

  if (pid == 0) {
    printf("[+] ----------- CHILD PID : %d ------------- \n", getpid());
    print_info();

    setuid(getuid());
    puts("[+] setuid(getuid()); was executed, result ...\n");
    print_info();

    puts("execv will be called, result ....\n");
    execv("./simpleProcess.out", NULL);
    return;
  }
  
  wait(status);
  printf("\n\n[+] ----------- MAIN PID : %d ------------- \n", getpid());
  print_info();

  return 0;
}

