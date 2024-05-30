#include "AmmoComponent.h"
#include "WorldFixture.cpp"
#include "FPSCharacter.h"
#include "Tests/AutomationCommon.h"
#include <Tests/AutomationEditorCommon.h>

BEGIN_DEFINE_SPEC(BPAmmoComponentSpecTest, "AutomationTestingExample.SpecTest.FPSCharacterBP.AmmoComponent", EAutomationTestFlags::ProductFilter | EAutomationTestFlags::ApplicationContextMask)
TWeakObjectPtr<AFPSCharacter> Character;
TSoftClassPtr<AFPSCharacter> ActorBPClass;
UAmmoComponent* ammoComponent;
UWorld* world;
int maxAmmo;
int ammoCount;
END_DEFINE_SPEC(BPAmmoComponentSpecTest)

void BPAmmoComponentSpecTest::Define()
{
	Describe("Ammo Component Checks", [this]()
		{
			BeforeEach([this]() {
				world = UWorld::CreateWorld(EWorldType::Game, false);
				ActorBPClass = FSoftClassPath(TEXT("/Game/Blueprints/BP_FPSCharacter.BP_FPSCharacter_C"));
				if (world)
				{
					// Spawn the actor
					Character = world->SpawnActor<AFPSCharacter>(ActorBPClass.LoadSynchronous());

					TestNotNull("MyCharacter", Character.Get());
					ammoComponent = Character.Get()->ammoComponent;

					maxAmmo = ammoComponent->getMaxAmmo();
					ammoCount = ammoComponent->getAmmoCount();
				}
				});

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


			AfterEach([this]() {
				ammoComponent = nullptr;
				world->DestroyWorld(false);
				});
		});
}