# Knowledge-Concurrency

## What is Thread?
Thread is a unit of a process that is responsible for executing the application code. 

By default, every process has at least one thread which is responsible for executing the application code and that thread is called as Main Thread. So, every application by default is a single-threaded application.

## What is Multithreading?

A process can have multiple threads and each thread can perform a different task. In simple words, we can say that the three methods we define in our program can be executed by three different threads. 
The advantage is that the execution takes place simultaneously (at the same time).

See example code [here](001-Multithreading/Program.cs)

## Protecting Shared Resource in Multithreading Using Locking

In a multithreading application, it is very important for us to handle multiple threads for executing critical section code. For example, if multiple threads want to access the shared resource then we need to protect the shared resource from concurrent access otherwise we will get some inconsistency output. 

See example code [here](002-MultithreadingUsingLocking/Program.cs)

Let us understand this with an example. In the following example, we are only protecting the shared Count variable from concurrent access.