namespace UnitTests
{
    [SupportedOSPlatform("windows")]
    public class MenuTests
    {
        [Test]
        [TestCase(0, 0, "> Item 0 <")]
        [TestCase(1, 0, "Item 1")]
        [TestCase(1, 4, "> Item 1 <")]
        [TestCase(2, 0, "Item 2")]
        [TestCase(2, 2, "> Item 2 <")]
        public void ConstructorAndIndexerMethodTest(int num, int selectorOn, string expected)
        {
            Menu menu = new Menu(new Window.ColoredString("Test Menu"),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Item 0"), () => { }),
                (new Window.ColoredString("Item 1"), () => { }),
                (new Window.ColoredString("Item 2"), () => { })
            }, selectorOn);
            string test = menu[num].String;
            Assert.That(test, Is.EqualTo(expected));
        }


        [Test]
        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(3)]
        public void IndexerBreakTest(int num)
        {
            Menu menu = new Menu(new Window.ColoredString("Test Menu"),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Item 0"), () => { })
            });
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                var t = menu[num];
            });
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(4, 0)]
        [TestCase(7, 3)]
        [TestCase(-2, 2)]
        [TestCase(-4, 0)]
        public void SelectItemAndConfirmMethodsTest(int num, int expected)
        {
            int test = -1;
            Menu menu = new Menu(new Window.ColoredString("Test Menu"),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Item 0"), () => test = 0),
                (new Window.ColoredString("Item 1"), () => test = 1),
                (new Window.ColoredString("Item 2"), () => test = 2),
                (new Window.ColoredString("Item 3"), () => test = 3)
            });
            menu.SelectItem(num);
            menu.Confirm();
            Assert.That(test, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 0)]
        [TestCase(10, 1)]
        [TestCase(23, 2)]
        [TestCase(55, 1)]
        public void AfterAndConfirmMethodsTest(int times, int expected)
        {
            int test = -1;
            Menu menu = new Menu(new Window.ColoredString("Test Menu"),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Item 0"), () => test = 0),
                (new Window.ColoredString("Item 1"), () => test = 1),
                (new Window.ColoredString("Item 2"), () => test = 2)
            });
            for (int i = 0; i < times; i++)
            {
                menu.After();
            }
            menu.Confirm();
            Assert.That(test, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 4)]
        [TestCase(4, 1)]
        [TestCase(7, 3)]
        [TestCase(10, 0)]
        [TestCase(21, 4)]
        [TestCase(48, 2)]
        public void BeforeAndConfirmMethodsTest(int times, int expected)
        {
            int test = -1;
            Menu menu = new Menu(new Window.ColoredString("Test Menu"),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Item 0"), () => test = 0),
                (new Window.ColoredString("Item 1"), () => test = 1),
                (new Window.ColoredString("Item 2"), () => test = 2),
                (new Window.ColoredString("Item 3"), () => test = 3),
                (new Window.ColoredString("Item 4"), () => test = 4)
            });
            for (int i = 0; i < times; i++)
            {
                menu.Before();
            }
            menu.Confirm();
            Assert.That(test, Is.EqualTo(expected));
        }

        [Test]
        public void AfterBeforeAndConfirmMethodsTest()
        {
            int test = -1;
            Menu menu = new Menu(new Window.ColoredString("Test Menu"),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Item 0"), () => test = 0),
                (new Window.ColoredString("Item 1"), () => test = 1),
                (new Window.ColoredString("Item 2"), () => test = 2)
            }, 2);

            menu.After();
            menu.Before();
            menu.Confirm();
            Assert.That(test, Is.EqualTo(2));
        }
    }
}