using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PatchMaker.Tests
{
    
    [TestClass]
    public class XElementExtensionsTests
    {
        [TestMethod]
        public void XElementExtensions_WithoutNestedQuery_FirstPathSegmentIsCorrect()
        {
            var path = "/sitecore[@database='SqlServer']/events[@timingLevel='custom']/event[@name='item:saved']/handler[@type='Sitecore.ExperienceAnalytics.Client.Deployment.Events.SegmentDeployedEventHandler, Sitecore.ExperienceAnalytics.Client' and @method='OnItemSaved']/param[@type='Sitecore.ExperienceAnalytics.Client.Deployment.DeploySegmentDefinitionProcessor, Sitecore.ExperienceAnalytics.Client']";

            var idx = path.LastPathSegmentIndex();
            var parent = path.FirstPathSegment(idx);
            var remainder = path.RemainingPathSegments(idx);

            Assert.AreEqual("/sitecore[@database='SqlServer']/events[@timingLevel='custom']/event[@name='item:saved']/handler[@type='Sitecore.ExperienceAnalytics.Client.Deployment.Events.SegmentDeployedEventHandler, Sitecore.ExperienceAnalytics.Client' and @method='OnItemSaved']", parent);
            Assert.AreEqual("param[@type='Sitecore.ExperienceAnalytics.Client.Deployment.DeploySegmentDefinitionProcessor, Sitecore.ExperienceAnalytics.Client']", remainder);
        }

        [TestMethod]
        public void XElementExtensions_WithNestedQuery_FirstPathSegmentIsCorrect()
        {
            var path = @"/sitecore[@database='SqlServer']/events[@timingLevel=""/test/blah[a='1']""]";

            var idx = path.LastPathSegmentIndex();
            var parent = path.FirstPathSegment(idx);
            var remainder = path.RemainingPathSegments(idx);

            Assert.AreEqual(@"/sitecore[@database='SqlServer']", parent);
            Assert.AreEqual(@"events[@timingLevel=""/test/blah[a='1']""]", remainder);
        }
    }

}