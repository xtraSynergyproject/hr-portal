import { useEffect, useState } from 'react'
import Box from '@mui/material/Box'
import { Button } from '@mui/material'

const NoteAudioAttachment = () => {
  const [audioURL, setAudioURL] = useState('')
  const [isRecording, setIsRecording] = useState(false)
  const [recorder, setRecorder] = useState(null)

  useEffect(() => {
    // Lazily obtain recorder first time we're recording.
    if (recorder === null) {
      if (isRecording) {
        requestRecorder().then(setRecorder, console.error)
      }
      return
    }

    // Manage recorder state.
    if (isRecording) {
      recorder.start()
    } else {
      recorder.stop()
    }

    // Obtain the audio when ready.
    const handleData = e => {
      setAudioURL(URL.createObjectURL(e.data))
    }

    recorder.addEventListener('dataavailable', handleData)
    return () => recorder.removeEventListener('dataavailable', handleData)
  }, [recorder, isRecording])

  const startRecording = () => {
    setIsRecording(true)
  }

  const stopRecording = () => {
    setIsRecording(false)
  }

  return (
    <Box sx={{border:"2px #D3D3D3 dashed", display: 'flex',alignItems:"center",flexDirection:"column", height:"200px" , p:"20px"}}>
        <Box sx={{height:"150px",width: '400px' ,  display: 'flex',alignItems:"center", justifyContent:"center"}}>
          <audio src={audioURL} controls />
        </Box>
        <Box>
          <Button onClick={startRecording} sx={{width :"100px", height:"50px", mx:"5px"}} variant='contained' disabled={isRecording}>
            start 
          </Button>
          <Button onClick={stopRecording} sx={{width :"100px", height:"50px", mx:"5px"}}  variant='contained' disabled={!isRecording}>
            stop 
          </Button>
        </Box>
    </Box>
  )
}

async function requestRecorder() {
  const stream = await navigator.mediaDevices.getUserMedia({ audio: true })
  return new MediaRecorder(stream)
}
export default NoteAudioAttachment
