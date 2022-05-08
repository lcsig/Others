#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/wait.h>

int main(){
    int id ;
    id = fork() ;
 
   if ( id < 0 ) {
      printf("[-] Fork Failed ...");
   }
   else if ( id == 0 ) {
      // execlp("/bin/ls", "ls", NULL);
      printf ( "[!!] Child  : Hello I am the child process\n");
      printf ( "[+]  Child  : Parent’s PID: %d\n", getppid());
      printf ( "[+]  Child  : Current PID: %d\n", getpid());
      printf ( "[+]  Child  : %d will be returned from exit\n", 177);
      // Only the lower 8 bits are available (0 - 255).
      exit(177); 
   }
   else {
      int status;
      int cpid = wait(&status);
      puts("");
      printf ( "[!!] Child has been terminated ...\n"); 
      printf ( "[+]  Parent : Hello I am the parent process\n" ) ;
      printf ( "[+]  Parent : Parent’s PID: %d\n", getppid());
      printf ( "[+]  Parent : Current PID: %d\n", getpid());
      printf ( "[+]  Parent : Child’s PID: %d\n", id);
      printf ( "[+]  Parent : Wait syscall has returned : %d\n", cpid);
      if ( WIFEXITED(status) ) // If returned normally ... 
         printf ( "[+]  Parent : Returned status is %d\n", WEXITSTATUS(status));
      
   }
 
}

