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

## Protecting Shared Resource in Multithreading Using  Mutex

Mutex also works likes a lock i.e. acquired an exclusive lock on a shared resource from concurrent access, but it works across multiple processes (if use named mutex). As we already discussed exclusive locking is basically used to ensure that at any given point of time, only one thread can enter into the critical section.

See example code [here](003-MultithreadingUsingMutex/Program.cs)

## What is Semaphore?

The Semaphore is used to limit the number of threads that can have access to a shared resource concurrently. In other words, we can say that Semaphore allows one or more threads to enter into the critical section and execute the task concurrently with thread safety. So, in real-time, we need to use Semaphore when we have a limited number of resources and we want to limit the number of threads that can use it.

See example code [here](004-Semaphore/Program.cs)

As you can see in the above example, we are passing two values to the constructor of the Semaphore class while initializing. These two values represent InitialCount and MaximumCount.

- InitialCount parameter  defines the initial number of requests for the semaphore that can be granted concurrently. 
- MaximumCount parameter defines the maximum number of requests for the semaphore that can be granted concurrently.

## What is a Deadlock?

We can define a deadlock is a situation where two or more threads are unmoving or frozen in their execution because they are waiting for each other to finish.

See example code [here](005-Deadlock/Program.cs)

In the following example, 
- For thread1, Account1001 is resource1 and Account1002 is resource2. 
- For thread2, Account1002 is resource1 and Account1001 is resource2.

The deadlock occurred when thread1 wants to acquire a lock on Account1001 which is already locked by thread2. Similarly thread2 wants to acquire a lock on Account1002 which is already locked by thread1.

One of the solution to handle deadlock is use try attempt to acquired lock

See example code [here](006-DeadlockSolution/Program.cs)

## Concurrency Control

### Optimistic

Optimistic Concurrency Control assumes that nothing's going to change while you're reading it.

- Check if the record was updated by someone else before you commit the transaction.
- If record dirty abort the transaction.

Optimistic locking is useful if the possibility for conflicts is very low. Need to cautious of the fact that how will application recover from these failures.

See example code [here](007-OptimisticConcurrencyControl/Program.cs)

### Pessimistic

Pessimistic Concurrency Control assumes that something will and so locks it.

- Take an exclusive lock so that no one else can start modifying the record.
- Others have to wait until the lock is released.
- Can caused deadlock.

Pessimistic locking is useful if there are a lot of updates and relatively high chances of users trying to update data at the same time.

See example code [here](008-PessimisticConcurrencyControl/Program.cs)
