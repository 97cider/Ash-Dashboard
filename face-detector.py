from keras.models import load_model
from keras.utils import CustomObjectScope
import tensorflow as tf
import cv2
import time
import numpy as np
from skimage.transform import resize
import dlib
from PIL import Image
import zmq as zatomapqueue
import face_pb2 as zatomsg
import argparse
parser = argparse.ArgumentParser(description="Rudamentary publisher for face data.")
parser.add_argument("--debug", help='Output verbose debug information including shapes and bounding boxes.', action='store_true')
args = parser.parse_args()
print(args.debug)
with CustomObjectScope({'tf': tf}):
  # model = load_model('./model/nn4.small2.v1.h5')
  # model.summary()

  ctx = zatomapqueue.Context()
  sock = ctx.socket(zatomapqueue.PUB)
  sock.setsockopt(zatomapqueue.SNDTIMEO,1000)
  sock.setsockopt(zatomapqueue.LINGER,1000)
  sock.setsockopt(zatomapqueue.IMMEDIATE,1)
  sock.bind("tcp://127.0.0.1:1339")

  default_camera_device = cv2.VideoCapture(0)
  key = cv2.waitKey(10)

  detector = dlib.get_frontal_face_detector()
  if (args.debug):
      win = dlib.image_window()
      win.clear_overlay()
  while (True):
      _, frame = default_camera_device.read()

      key = cv2.waitKey(10)

      # Do classification here
      # Single channel image -- Use for Face embeddings
      # img = np.around(np.transpose(resize(frame, (96,96)), (0,1,2))/255)

      # Convert to RGB
      image = Image.fromarray((frame).astype(np.uint8))
      image.convert('RGB')
      pimg2 = np.asarray(image)
      # Get facial embeddings. May be usefull?
      #pred = model.predict_on_batch(np.expand_dims(img, axis=0))

      # Create pb msg
      fmsg = zatomsg.Face()
      face_res = detector(pimg2, 1)
      if (args.debug):
          win.set_image(pimg2)
          win.add_overlay(face_res)

      if (len(face_res) != 0):
          print("Face found!")
          fmsg.isFace = 1
      else:
          fmsg.isFace = 0
      st = fmsg.SerializeToString()
      sock.send(st)
      if (args.debug):
          win.clear_overlay()
      time.sleep(1)
