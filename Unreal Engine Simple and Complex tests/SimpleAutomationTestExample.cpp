#include "FPSCharacter.h"
#include "AmmoComponent.h"

IMPLEMENT_SIMPLE_AUTOMATION_TEST(SimpleAmmoComponentMaxAmmoTest, "AutomationTestingExample.SimpleAndComplexAutomationTests.Simple.FPSCharacter.AmmoComponent.MaxAmmo", EAutomationTestFlags::EditorContext | EAutomationTestFlags::EngineFilter)
IMPLEMENT_SIMPLE_AUTOMATION_TEST(SimpleAmmoComponentAmmoCountTest, "AutomationTestingExample.SimpleAndComplexAutomationTests.Simple.FPSCharacter.AmmoComponent.AmmoCount", EAutomationTestFlags::EditorContext | EAutomationTestFlags::EngineFilter)
IMPLEMENT_SIMPLE_AUTOMATION_TEST(SimpleAmmoComponentAmmoCountAfterFireTest, "AutomationTestingExample.SimpleAndComplexAutomationTests.Simple.FPSCharacter.AmmoComponent.AmmoCountAfterFire", EAutomationTestFlags::EditorContext | EAutomationTestFlags::EngineFilter)

bool SimpleAmmoComponentMaxAmmoTest::RunTest(const FString& Parameters)
{
	bool result = false;
	int expectedMaxAmmo = 30;

	UWorld* world = UWorld::CreateWorld(EWorldType::Game, false);
	AFPSCharacter* character = world->SpawnActor<AFPSCharacter>();
	if (!character)
	{
		AddError(TEXT("Error, character is null"));
	}

	UAmmoComponent* ammoComponent = character->getAmmoComponent();
	int maxAmmo = ammoComponent->maxAmmo;

	if (maxAmmo == expectedMaxAmmo)
	{
		result = true;
	}
	else
	{
		AddError(TEXT("Error, Max ammo does not equal 30"));
	}

	return result;
}

bool SimpleAmmoComponentAmmoCountTest::RunTest(const FString& Parameters)
{
	bool result = false;
	int expectedAmmoCount = 30;

	UWorld* world = UWorld::CreateWorld(EWorldType::Game, false);
	AFPSCharacter* character = world->SpawnActor<AFPSCharacter>();
	if (!character)
	{
		AddError(TEXT("Error, character is null"));
	}

	UAmmoComponent* ammoComponent = character->getAmmoComponent();
	int ammoCount = ammoComponent->ammoCount;

	if (ammoCount == expectedAmmoCount)
	{
		result = true;
	}
	else
	{
		AddError(TEXT("Error, ammoCount does not equal 30"));
	}

	return result;
}

bool SimpleAmmoComponentAmmoCountAfterFireTest::RunTest(const FString& Parameters)
{
	bool result = false;
	int expectedAmmoCountAfterShotFired = 29;

	UWorld* world = UWorld::CreateWorld(EWorldType::Game, false);
	AFPSCharacter* character = world->SpawnActor<AFPSCharacter>();
	if (!character)
	{
		AddError(TEXT("Error, character is null"));
	}

	UAmmoComponent* ammoComponent = character->getAmmoComponent();
	int ammoCount = ammoComponent->ammoCount;

	character->Fire();
	ammoCount = ammoComponent->ammoCount;
	if (ammoCount == expectedAmmoCountAfterShotFired)
	{
		result = true;
	}
	else
	{
		AddError(TEXT("Error, ammoCount does not equal 29 after firing a shot"));
	}

	return result;
}
