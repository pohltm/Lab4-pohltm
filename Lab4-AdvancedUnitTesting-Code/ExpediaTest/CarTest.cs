using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;
using System.Collections.Generic;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}

        [Test()]
        public void TestThatCarDoesGetLocationFromTheDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            String carLocation = "My Garage";
            String anotherCarLocation = "Stelvio Pass";

            using (mocks.Record())
            {
                // The mock will return "My Garage" when the call is made with 15
                mockDatabase.getCarLocation(15);
                LastCall.Return(carLocation);

                // The mock will return "Stelvio Pass" when the call is made with 23
                mockDatabase.getCarLocation(23);
                LastCall.Return(anotherCarLocation);
            }

            var target = new Car(10);
            target.Database = mockDatabase;

            String result;

            result = target.getCarLocation(15);
            Assert.AreEqual(result, carLocation);

            result = target.getCarLocation(23);
            Assert.AreEqual(result, anotherCarLocation);

        }

        [Test()]
        public void TestThatCarDoesGetMileageFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            Int32 Miles = 103000;

            mockDatabase.Miles = Miles;

            var target = new Car(10);
            target.Database = mockDatabase;

            int mileage = target.Mileage;
            Assert.AreEqual(mileage, Miles);
        }

        [Test()]
        public void TestThatCarHasCorrectBasePriceForTenDaysWithObjectMother()
        {
            var target = ObjectMother.BMW();
            Assert.AreEqual(80, target.getBasePrice());
        }
	}
}
