import System.Environment

main = do
    env <- lookupEnv "TEST"

    putStrLn $ "Hello, " ++ maybe "test" id env