// This code is copyrighted by me :)


#include "SMainMenuWidget.h"
#include "Framework/MetaData/DriverMetaData.h"
#include <Kismet/GameplayStatics.h>

void SMainMenuWidget::Construct(const FArguments& InArgs)
{
	const FMargin ContentPadding = FMargin(500.f, 300.f);

	ChildSlot
		[
			SNew(SOverlay)
			+SOverlay::Slot()
			.HAlign(HAlign_Fill)
			.VAlign(VAlign_Fill)
			[
				SNew(SImage)
				.ColorAndOpacity(FColor::White)
			]
			+ SOverlay::Slot()
			.HAlign(HAlign_Fill)
			.VAlign(VAlign_Fill)
			.Padding(ContentPadding)
			[
				SNew(SVerticalBox)
				+SVerticalBox::Slot()
				[
					SNew(STextBlock)
					.Text(FText::FromString(TEXT("Game Title")))
				]
				+SVerticalBox::Slot()
				[
					SNew(SButton)
						.Text(FText::FromString(TEXT("ButtonText")))
						.AddMetaData(FDriverMetaData::Id("PlayButton"))
						.OnClicked(FOnClicked::CreateRaw(this, &SMainMenuWidget::TestButtonOnClick))  //(&SMainMenuWidget::TestButtonOnClick)
				]
			]
		];
}

FReply SMainMenuWidget::TestButtonOnClick()
{
	UWorld* world = nullptr;
	if (GEngine) world = GEngine->GameViewport->GetWorld();
	UGameplayStatics::OpenLevel(world, FName("FPSMap"));
	return FReply::Handled();
}