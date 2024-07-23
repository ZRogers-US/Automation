// This code is copyrighted by me :)


BEGIN_DEFINE_SPEC(BeforeEachTest, "AutomationDriver.BeforeEach.Test", EAutomationTestFlags::ProductFilter | EAutomationTestFlags::ApplicationContextMask)
FString TestString;
END_DEFINE_SPEC(BeforeEachTest)

void BeforeEachTest::Define()
{
    xBeforeEach([this]() 
    {
        TestString = "a";
        UE_LOG(LogTemp, Warning, TEXT("%s"), *TestString);
    });

    Describe("Sum testing", [this]()
    {
        xBeforeEach([this]() {
            TestString += " A";
            UE_LOG(LogTemp, Warning, TEXT("%s"), *TestString);
        });

        Describe("nested describe", [this]() 
        {
            BeforeEach([this]() {
                TestString += " A2";
                UE_LOG(LogTemp, Warning, TEXT("%s"), *TestString);
                });

            It("Should add first it", [this]() {
                TestString += " B";
                UE_LOG(LogTemp, Warning, TEXT("%s"), *TestString);
                });


            AfterEach([this]() {
                TestString += " C2";
                UE_LOG(LogTemp, Warning, TEXT("%s"), *TestString);
                });
        });

        It("Should add second it", [this]() {
            TestString += " B2";
            UE_LOG(LogTemp, Warning, TEXT("%s"), *TestString);
            });

        AfterEach([this]() {
            TestString += " C";
            UE_LOG(LogTemp, Warning, TEXT("%s"), *TestString);
        });
    });

    xAfterEach([this]() {
        TestString += " c";
        UE_LOG(LogTemp, Warning, TEXT("%s"), *TestString);
    });
}