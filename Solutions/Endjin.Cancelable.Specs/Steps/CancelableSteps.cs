namespace Endjin.Cancelable.Specs.Steps
{
    #region Using directives

    using System;
    using System.Threading.Tasks;

    using Endjin.Contracts;
    using Endjin.Core.Composition;
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
            var cancelable = ApplicationServiceLocator.Container.Resolve<ICancelable>();

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
            var cancelable = ApplicationServiceLocator.Container.Resolve<ICancelable>();
            IPeriodicityStrategy periodicityStrategy;

            ScenarioContext.Current.TryGetValue("PeriodicityStrategy", out periodicityStrategy);
            var delay = ScenarioContext.Current.Get<TimeSpan>("TaskTimeToComplete");
            var cancellationToken = ScenarioContext.Current.Get<string>("CancellationToken");

            var result = cancelable.RunUntilCompleteOrCancelledAsync(async token => Task.Delay(delay).Wait(), cancellationToken, periodicityStrategy).Result;

            ScenarioContext.Current.Set(result, "Result");
        }
    }
}