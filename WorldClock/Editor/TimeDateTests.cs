using NUnit.Framework;

public class TimeDateTests
{
    TimeData testData;
    GlobalTime testGlobal;
    int tickValue;

    [SetUp]
    public void SetUp()
    {
        testGlobal = new GlobalTime();
        testGlobal.minutesInAnHour = 60;
        testGlobal.hoursInADay = 6;
        testGlobal.daysInAMonth = 10;
        testGlobal.monthsInAYear = 3;
        testData = new TimeData(5, 1, 1, 1, 1, testGlobal);
        tickValue = 5;
    }

    #region Tick Turnover
    // A Test behaves as an ordinary method
    [Test]
    public void TimeDate_Tick5_AddsFiveMinutes()
    {
        //Arrange
        //Act
        testData.TickMinutes(tickValue);

        //Assert
        Assert.AreEqual(testData.Minutes, 10);
    }
    // A Test behaves as an ordinary method
    [Test]
    public void TimeDate_Tick5_MakesHourTurnover()
    {
        //Arrange
        testData = new TimeData(55, 1, 1, 1, 1, testGlobal);
        int hrs = testData.Hours;

        //Act
        testData.TickMinutes(tickValue);

        //Assert
        Assert.AreEqual(testData.Minutes, 0);
        Assert.AreEqual(testData.Hours, hrs + 1);
    }

    [Test]
    public void TimeDate_Tick5_MakesDayTurnover()
    {
        //Arrange
        testData = new TimeData(55, testGlobal.hoursInADay - 1, 1, 1, 1, testGlobal);
        int days = testData.Days;

        //Act
        testData.TickMinutes(tickValue);

        //Assert
        Assert.AreEqual(testData.Minutes, 0);
        Assert.AreEqual(testData.Hours, 0);
        Assert.AreEqual(testData.Days, days + 1);
    }
    [Test]
    public void TimeDate_Tick5_MakesMonthTurnover()
    {
        //Arrange
        testData = new TimeData(testGlobal.minutesInAnHour - tickValue, testGlobal.hoursInADay - 1, testGlobal.daysInAMonth - 1, 1, 1, testGlobal);
        int month = testData.Months;

        //Act
        testData.TickMinutes(tickValue);

        //Assert
        Assert.AreEqual(0, testData.Minutes);
        Assert.AreEqual(0, testData.Hours);
        Assert.AreEqual(0, testData.Days);
        Assert.AreEqual(month + 1, testData.Months);
    }
    [Test]
    public void TimeDate_Tick5_MakesYearTurnover()
    {
        //Arrange
        testData = new TimeData(55, testGlobal.hoursInADay - 1, testGlobal.daysInAMonth - 1, testGlobal.monthsInAYear - 1, 1, testGlobal);
        int year = testData.Years;

        //Act
        testData.TickMinutes(tickValue);

        //Assert
        Assert.AreEqual(testData.Minutes, 0);
        Assert.AreEqual(testData.Hours, 0);
        Assert.AreEqual(testData.Days, 0);
        Assert.AreEqual(testData.Months, 0);
        Assert.AreEqual(testData.Years, year + 1);
    }
    #endregion

    #region TickEvents
    [Test]
    public void TimeDate_Tick5_TickEvent()
    {
        //Arrange
        bool wasCalled = false;
        testData.Tick += () => wasCalled = true;

        //Act
        testData.TickMinutes(tickValue);

        //Assert
        Assert.IsTrue(wasCalled);
    }
    // A Test behaves as an ordinary method
    [Test]
    public void TimeDate_Tick5_NewHourEvent()
    {
        //Arrange
        testData = new TimeData(55, 1, 1, 1, 1, testGlobal);
        int hrs = testData.Hours;

        bool wasCalled = false;
        testData.NewHour += () => wasCalled = true;

        //Act
        testData.TickMinutes(tickValue);

        //Assert
        Assert.IsTrue(wasCalled);
    }

    [Test]
    public void TimeDate_Tick5_NewDayEvent()
    {
        //Arrange
        testData = new TimeData(55, testGlobal.hoursInADay - 1, 1, 1, 1, testGlobal);
        int days = testData.Days;
        bool wasCalled = false;
        testData.NewDay += () => wasCalled = true;

        //Act
        testData.TickMinutes(tickValue);

        //Assert
        Assert.IsTrue(wasCalled);
    }
    [Test]
    public void TimeDate_Tick5_NewMonthEvent()
    {
        //Arrange
        testData = new TimeData(55, testGlobal.hoursInADay - 1, testGlobal.daysInAMonth - 1, 1, 1, testGlobal);
        int month = testData.Months;
        bool wasCalled = false;
        testData.NewMonth += () => wasCalled = true;

        //Act
        testData.TickMinutes(tickValue);

        //Assert
        Assert.IsTrue(wasCalled);
    }
    [Test]
    public void TimeDate_Tick5_NewYearEvent()
    {
        //Arrange
        testData = new TimeData(55, testGlobal.hoursInADay - 1, testGlobal.daysInAMonth - 1, testGlobal.monthsInAYear - 1, 1, testGlobal);
        int year = testData.Years;
        bool wasCalled = false;
        testData.NewYear += () => wasCalled = true;

        //Act
        testData.TickMinutes(tickValue);

        //Assert
        Assert.IsTrue(wasCalled);
    }
    #endregion

    #region AdvanceToNextDay
    [Test]
    public void TimeDate_AdvanceToNextDay_AddsADay()
    {
        //Arrange
        int day = testData.Days;

        //Act
        testData.AdvanceToNextDayAtTime(testData.Hours, testData.Minutes);

        //Assert
        Assert.AreEqual(testData.Days, day + 1);
    }

    [Test]
    public void TimeDate_AdvanceToNextDay_AdjustsTime()
    {
        //Arrange
        int newHour = 4;
        int newMinute = 45;

        //Act
        testData.AdvanceToNextDayAtTime(newHour, newMinute);

        //Assert
        Assert.AreEqual(testData.Hours, newHour);
        Assert.AreEqual(testData.Minutes, newMinute);
    }

    [Test]
    public void TimeDate_AdvanceToNextDay_HandlesNegative()
    {
        //Arrange
        int newHour = -4;
        int newMinute = -45;

        //Act
        testData.AdvanceToNextDayAtTime(newHour, newMinute);

        //Assert
        Assert.AreEqual(testData.Hours, 0);
        Assert.AreEqual(testData.Minutes, 0);
    }
    [Test]
    public void TimeDate_AdvanceToNextDay_FiresNewDayEvent()
    {
        //Arrange
        bool wasCalled = false;
        testData.NewDay += () => wasCalled = true;

        //Act
        testData.AdvanceToNextDayAtTime(0, 0);

        //Assert
        Assert.IsTrue(wasCalled);
    }
    #endregion

    #region IsPastTime
    [Test]
    public void TimeDate_IsPastTime_ReturnsTrueWhenMinutePast()
    {
        //Arrange
        TimeData future = testData.CopyAndAddTime(5, 0, 0, 0, 0);

        //Act
        bool value = testData.IsAtOrPastTime(future);

        //Assert
        Assert.IsTrue(value);
    }
    [Test]
    public void TimeDate_IsPastTime_ReturnsTrueWhenHourPast()
    {
        //Arrange
        TimeData future = testData.CopyAndAddTime(0, 1, 0, 0, 0);

        //Act
        bool value = testData.IsAtOrPastTime(future);

        //Assert
        Assert.IsTrue(value);
    }
    [Test]
    public void TimeDate_IsPastTime_ReturnsTrueWhenDayPast()
    {
        //Arrange
        TimeData future = testData.CopyAndAddTime(0, 0, 1, 0, 0);

        //Act
        bool value = testData.IsAtOrPastTime(future);

        //Assert
        Assert.IsTrue(value);
    }
    [Test]
    public void TimeDate_IsPastTime_ReturnsTrueWhenMonthPast()
    {
        //Arrange
        TimeData future = testData.CopyAndAddTime(0, 0, 0, 1, 0);

        //Act
        bool value = testData.IsAtOrPastTime(future);

        //Assert
        Assert.IsTrue(value);
    }
    [Test]
    public void TimeDate_IsPastTime_ReturnsTrueWhenYearPast()
    {
        //Arrange
        TimeData future = testData.CopyAndAddTime(0, 0, 0, 0, 1);

        //Act
        bool value = testData.IsAtOrPastTime(future);

        //Assert
        Assert.IsTrue(value);
    }
    [Test]
    public void TimeDate_IsPastTime_ReturnsFalseWhenBehindMinutes()
    {
        //Arrange
        TimeData past = new TimeData(testData.Minutes - 5, testData.Hours, testData.Days, testData.Months, testData.Years, testGlobal);

        //Act
        bool value = testData.IsAtOrPastTime(past);

        //Assert
        Assert.IsFalse(value);
    }
    [Test]
    public void TimeDate_IsPastTime_ReturnsFalseWhenBehindHours()
    {
        //Arrange
        TimeData past = new TimeData(testData.Minutes, testData.Hours - 1, testData.Days, testData.Months, testData.Years, testGlobal);

        //Act
        bool value = testData.IsAtOrPastTime(past);

        //Assert
        Assert.IsFalse(value);
    }
    [Test]
    public void TimeDate_IsPastTime_ReturnsFalseWhenBehindDay()
    {
        //Arrange
        TimeData past = new TimeData(testData.Minutes, testData.Hours, testData.Days - 1, testData.Months, testData.Years, testGlobal);

        //Act
        bool value = testData.IsAtOrPastTime(past);

        //Assert
        Assert.IsFalse(value);
    }
    [Test]
    public void TimeDate_IsPastTime_ReturnsFalseWhenBehindMonth()
    {
        //Arrange
        TimeData past = new TimeData(testData.Minutes, testData.Hours, testData.Days, testData.Months - 1, testData.Years, testGlobal);

        //Act
        bool value = testData.IsAtOrPastTime(past);

        //Assert
        Assert.IsFalse(value);
    }
    [Test]
    public void TimeDate_IsPastTime_ReturnsFalseWhenBehindYear()
    {
        //Arrange
        TimeData past = new TimeData(testData.Minutes, testData.Hours, testData.Days, testData.Months, testData.Years - 1, testGlobal);

        //Act
        bool value = testData.IsAtOrPastTime(past);

        //Assert
        Assert.IsFalse(value);
    }
    [Test]
    public void TimeDate_IsPastTime_ReturnsTrueWhenEqual()
    {
        //Arrange
        //Act
        bool value = testData.IsAtOrPastTime(testData);

        //Assert
        Assert.IsTrue(value);
    }

    [Test]
    public void TimeDate_IsBehindCurrentTime_ReturnsTrueWhenBehind()
    {
        //Arrange
        TimeData past = new TimeData(testData.Minutes - 5, testData.Hours, testData.Days, testData.Months, testData.Years, testGlobal);

        //Act
        bool value = testData.IsBehindCurrentTime(past);

        //Assert
        Assert.IsTrue(value);
    }
    [Test]
    public void TimeDate_IsBehindCurrentTime_ReturnsFalseWhenAhead()
    {
        //Arrange
        TimeData past = new TimeData(testData.Minutes + 5, testData.Hours, testData.Days, testData.Months, testData.Years, testGlobal);

        //Act
        bool value = testData.IsBehindCurrentTime(past);

        //Assert
        Assert.IsFalse(value);
    }
    [Test]
    public void TimeDate_IsBehindCurrentTime_ReturnsFalseWhenEqual()
    {
        //Arrange
        //Act
        bool value = testData.IsBehindCurrentTime(testData);

        //Assert
        Assert.IsFalse(value);
    }
    #endregion

}

