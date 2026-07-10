export interface Content {
  id: string;
  title: string;
  description: string;
  type: 'Image' | 'Video' | 'Text';
  isActive: boolean;
}

export interface CreateContentRequest {
  title: string;
  description: string;
  type: 'Image' | 'Video' | 'Text';
}

export interface UpdateContentRequest {
  id: string;
  title: string;
  description: string;
  type: 'Image' | 'Video' | 'Text';
}
