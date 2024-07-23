// This code is copyrighted by me :)

#pragma once

#include "SlateBasics.h"
#include "SlateExtras.h"

/**
 * 
 */
class SMainMenuWidget : public SCompoundWidget
{

public:

	SLATE_BEGIN_ARGS(SMainMenuWidget) {}
	SLATE_ARGUMENT(TWeakObjectPtr<class AMainMenuHUD>, OwningHUD)
	SLATE_END_ARGS()
	// every widget needs a construction function
	void Construct(const FArguments& InArgs);
	FReply TestButtonOnClick();
	//HUD that created the widget
	TWeakObjectPtr<class AMainMenuHUD> OwningHUD;

	virtual bool SupportsKeyboardFocus() const override { return true; };

};
