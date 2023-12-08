using FrontDeskApp;
using NUnit.Framework;
using System;

namespace BookigApp.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConstructorShouldWorkCorrectly()
        {
            Hotel hotel = new Hotel("Trimoncium", 4);
            Assert.IsNotNull(hotel);
            Assert.That(hotel.Category > 0);
            Assert.That(hotel.FullName == "Trimoncium");
            Assert.That(hotel.Rooms.Count == 0);
            Assert.That(hotel.Bookings.Count == 0);
            Assert.That(hotel.Turnover == 0);
        }
        [Test]
        public void ConstuctorShould_ThrowError_WhenNameIsNullOrWhiteSpace()
        {
            Hotel hotel;
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => hotel = new Hotel("", 5));
            string expectedMessage = "Value cannot be null.";
            Assert.AreEqual(expectedMessage, ex.Message);
            hotel = new Hotel("bro", 5);
            Assert.That(hotel.FullName == "bro");
        }
        [Test]
        public void ConstuctorShould_ThrowError_WhenCategoryIs_LessThan1AndMoreThan5()
        {
            Hotel hotel;
            ArgumentException ex = Assert.Throws<ArgumentException>(() => hotel = new Hotel("Bro", 6));
            string expectedMessage = "Value does not fall within the expected range.";
            Assert.AreEqual(expectedMessage, ex.Message);
            Assert.That(ex.GetType().Name == "ArgumentException");
            hotel = new Hotel("bro", 5);
            Assert.That(5 == hotel.Category);
        }
        [Test]
        public void AddingRoomShould_IncreaseRoomsCount()
        {
            Hotel hotel = new Hotel("Trimoncium", 4);
            Room room = new Room(4, 100);
            Room room2 = new Room(6, 222);
            int roomCountBefore = hotel.Rooms.Count;
            hotel.AddRoom(room);
            Assert.That(roomCountBefore, Is.EqualTo(0));
            int roomCountAfter = hotel.Rooms.Count;
            Assert.That(roomCountAfter, Is.EqualTo(1));
            hotel.AddRoom(room2);
            roomCountAfter = hotel.Rooms.Count;
            Assert.That(roomCountAfter, Is.EqualTo(2));
        }
        [Test]
        public void BookRoom_ThrowsException_WhenAdultsAreLessThan1()
        {
            Hotel hotel = new Hotel("Trimoncium", 4);
            Room room = new Room(4, 100);
            ArgumentException ex = Assert.Throws<ArgumentException>(() => hotel.BookRoom(0, 4, 4, 100));
            string expectedMessage = "Value does not fall within the expected range.";
            Assert.AreEqual(expectedMessage, ex.Message);
            Assert.That(ex.GetType().Name == "ArgumentException");
        }
        [Test]
        public void BookRoom_ThrowsException_WhenChildrenAreLessThan0()
        {
            Hotel hotel = new Hotel("Trimoncium", 4);
            Room room = new Room(4, 100);
            ArgumentException ex = Assert.Throws<ArgumentException>(() => hotel.BookRoom(2, -1, 4, 100));
            string expectedMessage = "Value does not fall within the expected range.";
            Assert.AreEqual(expectedMessage, ex.Message);
            Assert.That(ex.GetType().Name == "ArgumentException");
        }
        [Test]
        public void BookRoom_ThrowsException_WhenDurationIsLessThan1()
        {
            Hotel hotel = new Hotel("Trimoncium", 4);
            Room room = new Room(4, 100);
            hotel.AddRoom(room);
            ArgumentException ex = Assert.Throws<ArgumentException>(() => hotel.BookRoom(2, 1, 0, 100));
            string expectedMessage = "Value does not fall within the expected range.";
            Assert.AreEqual(expectedMessage, ex.Message);
            Assert.That(ex.GetType().Name == "ArgumentException");
        }
        [Test]
        public void BookRoom_Should_IncreaseBookingCount()
        {
            Hotel hotel = new Hotel("Trimoncium", 4);
            Room room = new Room(4, 55);
            hotel.AddRoom(room);
            Assert.That(hotel.Bookings.Count, Is.EqualTo(0));
            hotel.BookRoom(2, 1, 3, 1200);
            int expectedCount = 1;
            Assert.AreEqual(expectedCount, hotel.Bookings.Count);
            Room room2 = new Room(6, 111);
            hotel.AddRoom(room2);
            hotel.BookRoom(2, 3, 2, 1500);
            expectedCount = 2;
            Assert.AreEqual(expectedCount, hotel.Bookings.Count);
        }
        [Test]
        public void BookRoom_Should_IncreaseTurnover()
        {
            Hotel hotel = new Hotel("Trimoncium", 4);
            Room room = new Room(4, 50);
            hotel.AddRoom(room);
            Assert.That(hotel.Turnover, Is.EqualTo(0));
            hotel.BookRoom(2, 1, 3, 1200);
            Assert.That(hotel.Turnover > 0);
            Room room2 = new Room(6, 100);
            hotel.AddRoom(room2);
            hotel.BookRoom(2, 3, 2, 1500);
            double expectedTurnover = 650;
            Assert.AreEqual(350, hotel.Turnover);

        }
        [Test]
        public void BookRoom_Should_CheckRoomBedCapacity()
        {
            Hotel hotel = new Hotel("Trimoncium", 4);
            Room room = new Room(4, 55);
            hotel.AddRoom(room);
            hotel.BookRoom(2, 3, 3, 1200);
            int expectedCount = 0;
            Assert.AreEqual(expectedCount, hotel.Bookings.Count);
            Room room2 = new Room(6, 111);
            hotel.AddRoom(room2);
            hotel.BookRoom(3, 3, 2, 1500);
            expectedCount = 1;
            Assert.AreEqual(expectedCount, hotel.Bookings.Count);
        }

    }
}