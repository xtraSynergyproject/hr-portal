import { Folder } from '@mui/icons-material'
import { Button, Typography } from '@mui/material'
import { Box } from '@mui/system'
import axios from 'axios'
import { useEffect, useState } from 'react'
import ExpandMoreIcon from '@mui/icons-material/ExpandMore'
import ChevronRightIcon from '@mui/icons-material/ChevronRight'

const WorkspaceTreeview = ({ sourceFolder, setActive, active }) => {
  let [childFolders, setChildFolders] = useState([])
  let [expand, setExpand] = useState(false)

  const fetchChildFolders = async () => {
    if (sourceFolder === undefined) return
    try {
      const response = await axios.get(
        `https://webapidev.aitalkx.com/dms/query/GetChildFolders?key=${sourceFolder.key}&userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR`,
        {
          headers: {
            'Access-Control-Allow-Origin': '*'
          }
        }
      )
      setChildFolders(response.data)
    } catch (error) {
      console.log(error)
    }
  }

  const handleExpand = event => {
    setExpand(!expand)
    if (event.detail === 2) {
      setExpand(true)
      setActive(sourceFolder)
      alert('double clicked')
      console.log(active)
    }
  }

  useEffect(() => {
    fetchChildFolders()
  }, [])

  if (childFolders.length !== 0) {
    return (
      <div style={{ width: '300px' }}>
        <Box
          sx={{
            backgroundColor: sourceFolder.key === active?.key ? '#e2e8f0' : '',
            cursor: 'pointer',
            display: 'flex',
            gap: '10px',
            '&:hover': {
              backgroundColor: '#e2e8f0'
            }
          }}
          onClick={handleExpand}
        >
          {expand ? <ExpandMoreIcon /> : <ChevronRightIcon />}
          <Folder sx={{ color: sourceFolder.folder ? ' #FFB400' : '#9155FD' }} />
          {sourceFolder?.title}
          <span
            style={{
              paddingLeft: '20px',
              display: 'inline-block'
            }}
          >
            {/* {sourceFolder.Count} */}
          </span>
        </Box>

        <Box
          sx={{
            display: expand ? 'flex' : 'none',
            flexDirection: 'column',
            alignItems: 'flex-start',
            paddingLeft: '10px',
            gap: '5px'
          }}
        >
          {childFolders.map(childFolder => {
            return (
              <WorkspaceTreeview
                key={childFolder.key}
                active={active}
                setActive={setActive}
                sourceFolder={childFolder}
              />
            )
          })}
        </Box>
      </div>
    )
  } else {
    return (
      <div style={{ width: '300px' }}>
        <Box
          onClick={handleExpand}
          sx={{
            backgroundColor: sourceFolder.key === active?.key ? 'e2e8f0' : '',
            cursor: 'pointer',
            display: 'flex',
            gap: '10px',
            '&:hover': {
              backgroundColor: '#e2e8f0'
            }
          }}
        >
          {expand ? <ExpandMoreIcon /> : <ChevronRightIcon />}
          <Folder sx={{ color: sourceFolder.folder ? ' #FFB400' : '#9155FD' }} />
          {sourceFolder?.title}
          <span
            style={{
              paddingLeft: '20px',
              display: 'inline-block'
            }}
          >
            {/* {sourceFolder.Count} */}
          </span>
        </Box>
      </div>
    )
  }
}

export default WorkspaceTreeview
