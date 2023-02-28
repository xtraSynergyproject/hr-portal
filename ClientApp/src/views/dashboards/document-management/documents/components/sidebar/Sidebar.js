import { Box, CircularProgress } from '@mui/material'
import axios from 'axios'
import { useEffect, useState } from 'react'
import WorkspaceTreeview from './workspace-treeview/WorkspaceTreeview'

const Sidebar = () => {
  const [sourceFolders, setSourceFolders] = useState([])
  const [isloading, setIsLoading] = useState(false)
  const [active, setActive] = useState({})

  const fetchSourceFolders = async () => {
    setIsLoading(true)
    try {
      const response = await axios.get(
        'https://webapidev.aitalkx.com/dms/query/GetSourceFolders?userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR',
        {
          headers: {
            'Access-Control-Allow-Origin': '*'
          }
        }
      )
      setSourceFolders(response.data)
      setIsLoading(false)
    } catch (error) {
      setIsLoading(false)
      console.log(error)
    }
  }

  useEffect(() => {
    fetchSourceFolders()
  }, [])
  return (
    <Box width='300px'>
      {isloading ? (
        <CircularProgress />
      ) : (
        <div
          style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'flex-start',
            gap: '5px'
          }}
        >
          {sourceFolders.map(sourceFolder => {
            return (
              <WorkspaceTreeview
                key={sourceFolder.key}
                active={active}
                setActive={setActive}
                sourceFolder={sourceFolder}
              />
            )
          })}
        </div>
      )}
    </Box>
  )
}

export default Sidebar
