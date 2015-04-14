Feature: Cancelable
	In order to cancel long running tasks in a distributed system
	As an actor in the system
	I want to be able to issue a command to cancel a long running task

@container @storage_emulator
Scenario: A long running task runs to completion
	Given the long running task takes 3 seconds to complete
	And the task is told to cancel if the cancellation token '19d411fc-5014-4eaa-8f84-0fe3d4b93068' is issued
	And no cancelation token is issued
	When I execute the task 
	Then it should complete sucessfully

@container @storage_emulator
Scenario: A long running task is cancelled
	Given the long running task takes 6 seconds to complete
	And the task is told to cancel if the cancellation token '5c8f0e6a-ca54-4824-ac19-ccea64239a17' is issued
	And a cancelation token is issued
	When I execute the task
	Then the task should be cancelled

@container @storage_emulator
Scenario: A long running task runs to completion and is then cancelled
	Given the long running task takes 4 seconds to complete
	And the task is told to cancel if the cancellation token '5e3e3b9d-d42a-4554-97ab-f2dffee8a746' is issued
	And a cancelation token is issued
	When I execute the task 
	Then it should complete sucessfully

@container @storage_emulator
Scenario: A long running task runs to completion when an issued cancellation token is deleted prior to running
	Given the long running task takes 6 seconds to complete
	And that a cancellation token '5e3e3b9d-d42a-4554-97ab-f2dffee8a746' is issued
	And the cancellation token '5e3e3b9d-d42a-4554-97ab-f2dffee8a746' is deleted
	And the task is told to cancel if the cancellation token '5e3e3b9d-d42a-4554-97ab-f2dffee8a746' is issued
	When I execute the task 
	Then it should complete sucessfully

# We need to think about creating a system service for cleaning up old cancellation tokens to deal with the scenario of something being cancelled after it's actually
# finished running. We could do something like scan for blobs that are > 24 hours old and remove them.
