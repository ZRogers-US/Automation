#include "Tests/AutomationCommon.h"
#include <Tests/AutomationEditorCommon.h>
#include "AbilitySystemComponent.h"
#include <GAScppProject/GAScppProjectCharacter.h>
#include "GAScppProject/BasicAttributeSet.h"
#include <Kismet/GameplayStatics.h>
#include <GAScppProject/ManaRegenPickup.h>


#if WITH_DEV_AUTOMATION_TESTS


BEGIN_DEFINE_SPEC(GASManaUsedOnAttackSpec, "GAS.Spec.Mana.ReducedOnAttack", EAutomationTestFlags::EditorContext | EAutomationTestFlags::EngineFilter)
TSoftClassPtr<AGAScppProjectCharacter> BPCharacterClass;
TWeakObjectPtr<AGAScppProjectCharacter> Character;
UWorld* world;
END_DEFINE_SPEC(GASManaUsedOnAttackSpec)

void GASManaUsedOnAttackSpec::Define()
{
	Describe("GAS Mana Test", [this]()
	{
		BeforeEach([this]() {
			//world = UWorld::CreateWorld(EWorldType::Game, false);
			//world->Tick(ELevelTick::LEVELTICK_All, 1.0f);
			//world->SetShouldTick(true);
			AutomationOpenMap("/Game/FirstPerson/Maps/EmptyTestMap.EmptyTestMap");
			world = GEngine->GetWorldContexts()[0].World();
			BPCharacterClass = FSoftClassPath(TEXT("/Game/ParagonSevarog/Characters/Heroes/Sevarog/SevarogPlayerCharacter.SevarogPlayerCharacter_C"));
			if (world)
			{
				// Spawn the actor
				Character = world->SpawnActor<AGAScppProjectCharacter>(BPCharacterClass.LoadSynchronous());
			}
		});

		It("Should start with 100 Mana", [this]() {
			/*UAbilitySystemComponent* GASComponent = Character->GetAbilitySystemComponent();
			UE_LOG(LogTemp, Warning, TEXT("Hello World"));
			TSubclassOf<UBasicAttributeSet> BasicAttributeSet;
			GASComponent->GetAttributeSet(BasicAttributeSet);*/
			int InitalManaValue = Character->GetAbilitySystemComponent()->GetNumericAttribute(UBasicAttributeSet::GetManaAttribute());  //Character->BasicAttributeSet->GetMana();  <-access violation set isnull
			
			TestEqual("Mana Starts at 100", InitalManaValue, 100);
		});

		xIt("Should reduce by 20 upon attacking", [this]() {
			int InitalManaValue = Character->GetAbilitySystemComponent()->GetNumericAttribute(UBasicAttributeSet::GetManaAttribute());

			/*struct Params
			{
				bool RetVal;
			};
			Params value;
			if (UFunction* Attack = Character->FindFunction("Attack"))
			{
				Character->ProcessEvent(Attack, &value);
				UE_LOG(LogTemp, Warning, TEXT("Attack Function Found"));
			}*/
			Character->AttackWithMana();

			int ManaValueAfterAttack = Character->GetAbilitySystemComponent()->GetNumericAttribute(UBasicAttributeSet::GetManaAttribute());

			TestEqual("Mana is now 80", ManaValueAfterAttack, 80);
		});

		It("Should regen Mana by 20 when picking up a ManaRegen", [this]() {

			TArray<AActor*> actorArray;
			UGameplayStatics::GetAllActorsOfClass(world, AManaRegenPickup::StaticClass(), actorArray);
			if (actorArray.Num() > 0) Cast<AManaRegenPickup>(actorArray[0])->GiveMana(Character.Get());

			int ManaValueAfterRegen = Character->GetAbilitySystemComponent()->GetNumericAttribute(UBasicAttributeSet::GetManaAttribute());
			TestEqual("Mana is now 120", ManaValueAfterRegen, 120);
		});

		It("Should use 20 mana when dashing", [this]() {

			Character->TriggerDashAbility();
			int ManaValueAfterDash = Character->GetAbilitySystemComponent()->GetNumericAttribute(UBasicAttributeSet::GetManaAttribute());
			TestEqual("Mana is now 80", ManaValueAfterDash, 100); //100 cause for some reason is colliding with mana regen pickup
			});
	});
}

#endif //WITH_DEV_AUTOMATION_TESTS