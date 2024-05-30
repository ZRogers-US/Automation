// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "AmmoComponent.generated.h"


UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class FPSPROJECT_API UAmmoComponent : public USceneComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UAmmoComponent();

protected:
	// Called when the game starts
	virtual void BeginPlay() override;


public:	
	void ReduceAmmoCount(int value);

	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	int maxAmmo;

	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	int ammoCount;

	int getAmmoCount();
	int getMaxAmmo();
};
