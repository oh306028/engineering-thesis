import React from "react";
import LearningPathView, { type LearningViewProps } from "./learningPathView";

function Challenge({ onClose }: LearningViewProps) {
  return (
    <LearningPathView
      pathType="Challenge"
      title="Wyzwanie"
      subtitle="Sprawdź się w codziennym wyzwaniu i zdobądź dodatkowe punkty!"
      icon="🎯"
      onClose={onClose}
    />
  );
}

export default Challenge;
