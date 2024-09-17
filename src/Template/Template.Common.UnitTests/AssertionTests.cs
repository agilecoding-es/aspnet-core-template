using Template.Common.Assertions;

namespace Template.Common.UnitTests
{
    public class AssertionTests
    {
        [Fact]
        public void IsNull_ValueNotNull_ThrowsException()
        {
            // Arrange
            var obj = new object();
            var assertion = Assertion<InvalidOperationException>.This(obj);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => assertion.IsNull("Value is not null"));
        }

        [Fact]
        public void IsNull_ValueIsNull_DoesNotThrow()
        {
            // Arrange
            object obj = null;
            var assertion = Assertion<InvalidOperationException>.This(obj);

            // Act
            var result = assertion.IsNull("Value is null");

            // Assert
            Assert.NotNull(result); // Verifica que devuelve el propio objeto de Assertion
        }

        [Fact]
        public void IsEqual_ValuesAreNotEqual_ThrowsException()
        {
            // Arrange
            var obj1 = new object();
            var obj2 = new object();
            var assertion = Assertion<InvalidOperationException>.This(obj1);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => assertion.IsEqual(obj2, "Values are not equal"));
        }
    }
}