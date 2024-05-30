// Fill out your copyright notice in the Description page of Project Settings.


#include "AmmoComponent.h"

// Sets default values for this component's properties
UAmmoComponent::UAmmoComponent()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = false;

	maxAmmo = 30;
	ammoCount = maxAmmo;
	// ...
}


// Called when the game starts
void UAmmoComponent::BeginPlay()
{
	Super::BeginPlay();

	// ...	
}


void UAmmoComponent::ReduceAmmoCount(int value)
{
	ammoCount -= value;

}

int UAmmoComponent::getAmmoCount()
{
	return ammoCount;
}

int UAmmoComponent::getMaxAmmo()
{
	return maxAmmo;
}

