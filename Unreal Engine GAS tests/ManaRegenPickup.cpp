// Fill out your copyright notice in the Description page of Project Settings.


#include "ManaRegenPickup.h"
#include "GAScppProjectCharacter.h"
#include "AbilitySystemComponent.h"
#include "Components/SphereComponent.h"



// Sets default values
AManaRegenPickup::AManaRegenPickup()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;


	ColliderComponent = CreateDefaultSubobject<USphereComponent>(TEXT("ColliderComponent"));
	StaticMeshComponent = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("MeshComponent"));
	RootComponent = StaticMeshComponent;
}

// Called when the game starts or when spawned
void AManaRegenPickup::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void AManaRegenPickup::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void AManaRegenPickup::GiveMana(AGAScppProjectCharacter* other)
{
	UAbilitySystemComponent* playerAbilitySystem = other->GetAbilitySystemComponent(); //->ApplyGameplayEffectToSelf()/Script/Engine.Blueprint'/Game/FirstPerson/GE_RestoreMana.GE_RestoreMana'
	FGameplayEffectContextHandle ManaRegenContextHandle;
	UGameplayEffect* ManaRegenEffectPtr = ManaRegenEffect.GetDefaultObject();
	playerAbilitySystem->ApplyGameplayEffectToSelf(ManaRegenEffectPtr, 0, ManaRegenContextHandle);
}