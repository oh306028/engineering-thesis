import React from "react";
import LearningPathView, { type LearningViewProps } from "./learningPathView";

function Learning({ onClose }: LearningViewProps) {
  return (
    <LearningPathView
      pathType="Regular"
      title="Nauka"
      subtitle="Poszerzaj swoją wiedzę z codzienną dawką nauki!"
      icon="📚"
      onClose={onClose}
    />
  );
}

export default Learning;
