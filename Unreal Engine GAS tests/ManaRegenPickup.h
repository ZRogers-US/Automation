// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "ManaRegenPickup.generated.h"

UCLASS()
class GASCPPPROJECT_API AManaRegenPickup : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AManaRegenPickup();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

public:
	UPROPERTY(VisibleAnywhere, BlueprintReadWrite)
	class USphereComponent* ColliderComponent;
	UPROPERTY(VisibleAnywhere, BlueprintReadWrite)
	class UStaticMeshComponent* StaticMeshComponent;
	UPROPERTY(EditDefaultsOnly)
	TSubclassOf<class UGameplayEffect> ManaRegenEffect;

	UFUNCTION(BlueprintCallable)
	void GiveMana(class AGAScppProjectCharacter* other); // should be done as a gameplay effect 
};
