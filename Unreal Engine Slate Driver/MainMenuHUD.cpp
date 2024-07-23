// This code is copyrighted by me :)


#include "MainMenuHUD.h"
#include "SMainMenuWidget.h"
#include "Engine/Engine.h"

void AMainMenuHUD::BeginPlay()
{
	Super::BeginPlay();

	if (GEngine && GEngine->GameViewport)
	{
		MainMenuWidget = SNew(SMainMenuWidget).OwningHUD(this);
		GEngine->GameViewport->AddViewportWidgetContent(SAssignNew(MainMenuWidgetContainer, SWeakWidget).PossiblyNullContent(MainMenuWidget.ToSharedRef()));
	}
}
