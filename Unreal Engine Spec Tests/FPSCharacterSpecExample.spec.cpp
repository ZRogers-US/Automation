#include "AmmoComponent.h"
#include "WorldFixture.cpp"
#include "FPSCharacter.h"
#include <Tests/AutomationEditorCommon.h>


//Spec type Automation test to test the Ammo component on a C++ FPSCharacter class (projectile hard coded in the constuctor)

//Define the test and any varibles
BEGIN_DEFINE_SPEC(AmmoComponentSpecTest, "AutomationTestingExample.SpecTest.FPSCharacter.AmmoComponent", EAutomationTestFlags::ProductFilter | EAutomationTestFlags::ApplicationContextMask)
TUniquePtr<FWorldFixture> WorldFixture;
TWeakObjectPtr<AFPSCharacter> Character;
UAmmoComponent* ammoComponent;
int maxAmmo;
int ammoCount;
END_DEFINE_SPEC(AmmoComponentSpecTest)

//Start with the define function which contains the tests
void AmmoComponentSpecTest::Define()
{
	//Groups together tests
	Describe("Ammo Component Checks", [this]()
		{
			//function run before each test
			BeforeEach([this]() {
				WorldFixture = MakeUnique<FWorldFixture>(); // create a world fixture (class that creates a UWorld and manages the destuction of actors after the test)
				if (WorldFixture->GetWorld())
				{
					// Spawn the actor
					Character = WorldFixture->GetWorld()->SpawnActor<AFPSCharacter>();
					//ensure character is not null
					TestNotNull("MyCharacter", Character.Get());
					//get the ammo component from the character
					ammoComponent = Character.Get()->ammoComponent;
					//get teh values for ammo count and max ammo fromthe ammo component
					maxAmmo = ammoComponent->getMaxAmmo();
					ammoCount = ammoComponent->getAmmoCount();
				}
				});
			//It function is each individual test, test name should start with should
			It("Should start with a max ammo count of 30", [this]() {
				if (!TestNotNull("CharacterFixture object created in BeforeEach", Character.Get()))
					return;

				TestEqual("max ammo is 30", maxAmmo, 30);
				});

			It("Should start with an ammo count of 30", [this]() {
				if (!TestNotNull("CharacterFixture object created in BeforeEach", Character.Get()))
					return;

				TestEqual("ammo count is 30", ammoCount, 30);
				});

			It("Should have an ammo count of 29 after shooting once", [this]() {
				if (!TestNotNull("CharacterFixture object created in BeforeEach", Character.Get()))
					return;

				Character.Get()->Fire();
				ammoCount = ammoComponent->getAmmoCount();
				TestEqual("ammo count is now 29", ammoCount, 29);
				});

			//function run after each test
			AfterEach([this]() {
				ammoComponent = nullptr;
				WorldFixture.Reset();
				});
		});
}