// This code is copyrighted by me :)

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/HUD.h"
#include "MainMenuHUD.generated.h"

/**
 * 
 */
UCLASS()
class AMainMenuHUD : public AHUD
{
	GENERATED_BODY()
	

	TSharedPtr<class SMainMenuWidget> MainMenuWidget;
	TSharedPtr<class SWidget> MainMenuWidgetContainer;

	virtual void BeginPlay() override;
};
