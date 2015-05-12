namespace Endjin.Cancelable.Specs.Steps
{
    #region Using directives

    using System;
    using System.Threading.Tasks;

    using Endjin.Cancelable.Azure.Storage;
    using Endjin.Cancelable.Specs.Configuration;
    using Endjin.Core.Repeat.Strategies;

    using Should;

    using TechTalk.SpecFlow;

    #endregion

    [Binding]
    public class CancelableSteps
    {
        [Given(@"a cancelation token is issued")]
        public void GivenACancelationTokenIsIssued()
        {
            var cancelable = new Cancelable(new CancellationTokenProvider(new ConnectionStringProvider()), new CancellationTokenObserverFactory(new CancellationTokenProvider(new ConnectionStringProvider())));

            string token = ScenarioContext.Current.Get<string>("CancellationToken");
            cancelable.CreateTokenAsync(token).Wait();
        }

        [Given(@"no cancelation token is issued")]
        public void GivenNoCancelationTokenIsIssued()
        {
            // Do nothing
        }

        [Given(@"the long running task takes (.*) to complete")]
        public void GivenTheLongRunningTaskTakesToComplete(TimeSpan timeToComplete)
        {
            ScenarioContext.Current.Set(timeToComplete, "TaskTimeToComplete");
        }

        [Given(@"the task is told to cancel if the cancellation token '(.*)' is issued")]
        public void GivenTheTaskIsToldToCancelIfTheCancellationTokenIsIssued(string token)
        {
            ScenarioContext.Current.Set(token, "CancellationToken");
        }

        [Given(@"that a cancellation token '(.*)' is issued")]
        public void GivenThatACancellationTokenIsIssued(string token)
        {
            var cancelable = new Cancelable(new CancellationTokenProvider(new ConnectionStringProvider()), new CancellationTokenObserverFactory(new CancellationTokenProvider(new ConnectionStringProvider())));
            cancelable.CreateTokenAsync(token).Wait();
        }

        [Given(@"the cancellation token '(.*)' is deleted")]
        public void GivenTheCancellationTokenIsDeleted(string token)
        {
            var cancelable = new Cancelable(new CancellationTokenProvider(new ConnectionStringProvider()), new CancellationTokenObserverFactory(new CancellationTokenProvider(new ConnectionStringProvider())));
            cancelable.DeleteTokenAsync(token).Wait();
        }


        [Then(@"it should complete sucessfully")]
        public void ThenItShouldCompleteSucessfully()
        {
            var cancelableResult = ScenarioContext.Current.Get<CancelableResult>("Result");
            cancelableResult.ShouldEqual(CancelableResult.Completed);
        }

        [Then(@"the task should be cancelled")]
        public void ThenTheTaskShouldBeCancelled()
        {
            var cancelableResult = ScenarioContext.Current.Get<CancelableResult>("Result");
            cancelableResult.ShouldEqual(CancelableResult.Cancelled);
        }

        [When(@"I execute the task")]
        public void WhenIExecuteTheTask()
        {
            var cancelable = new Cancelable(new CancellationTokenProvider(new ConnectionStringProvider()), new CancellationTokenObserverFactory(new CancellationTokenProvider(new ConnectionStringProvider())));
            IPeriodicityStrategy periodicityStrategy;

            ScenarioContext.Current.TryGetValue("PeriodicityStrategy", out periodicityStrategy);
            var delay = ScenarioContext.Current.Get<TimeSpan>("TaskTimeToComplete");
            var cancellationToken = ScenarioContext.Current.Get<string>("CancellationToken");

            var result = cancelable.RunUntilCompleteOrCancelledAsync(async token => await Task.Delay(delay), cancellationToken, periodicityStrategy).Result;

            ScenarioContext.Current.Set(result, "Result");
        }
    }
}