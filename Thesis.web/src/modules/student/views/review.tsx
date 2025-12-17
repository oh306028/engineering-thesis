import React from "react";
import LearningPathView, { type LearningViewProps } from "./learningPathView";

function Review({ onClose }: LearningViewProps) {
  return (
    <LearningPathView
      pathType="Review"
      title="Powtórka"
      subtitle="Utrwal swoją wiedzę poprzez regularne powtórki!"
      icon="🔄"
      onClose={onClose}
    />
  );
}

export default Review;
